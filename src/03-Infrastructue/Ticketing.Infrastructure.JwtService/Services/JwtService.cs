using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.Contracts.Services;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.JwtService.Options;

namespace Ticketing.Infrastructure.JwtService.Services;

public sealed class JwtService : IJwtService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ILogger<JwtService> _logger;
    private readonly JwtOptions _options;
    private readonly SigningCredentials _signingCredentials;

    public JwtService(
        UserManager<AppUser> userManager,
        IRefreshTokenRepository refreshTokens,
        ILogger<JwtService> logger,
        IOptions<JwtOptions> options)
    {
        _userManager = userManager;
        _refreshTokens = refreshTokens;
        _logger = logger;
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.Issuer))
            throw new InvalidOperationException("JWT issuer is not configured.");
        if (string.IsNullOrWhiteSpace(_options.Audience))
            throw new InvalidOperationException("JWT audience is not configured.");

        var keyBytes = Encoding.UTF8.GetBytes(_options.SigningKey);
        if (keyBytes.Length < 32)
            throw new InvalidOperationException("JWT signing key must contain at least 32 UTF-8 bytes for HS256.");
        if (_options.AccessTokenExpirationMinutes <= 0)
            throw new InvalidOperationException("JWT access token expiration must be greater than zero.");
        if (_options.RefreshTokenExpirationDays <= 0)
            throw new InvalidOperationException("JWT refresh token expiration must be greater than zero.");

        _signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256);
    }

    public async Task<AuthTokenDto> CreateTokenPairAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();

        var now = DateTime.UtcNow;
        var refresh = CreateRefreshToken(user.Id, Guid.NewGuid(), now);
        await _refreshTokens.AddAsync(refresh.Entity, cancellationToken);
        await _refreshTokens.SaveChangesAsync(cancellationToken);

        return await CreateTokenPairAsync(user, refresh.RawToken, refresh.Entity.ExpiresAtUtc, now);
    }

    public async Task<AuthTokenDto?> RefreshTokenPairAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var current = await _refreshTokens.GetByHashAsync(HashToken(refreshToken), cancellationToken);
        if (current is null)
            return null;

        var now = DateTime.UtcNow;
        if (current.ExpiresAtUtc <= now)
            return null;

        if (current.RevokedAtUtc is not null)
        {
            _logger.LogWarning("Detected reuse of revoked refresh token for token family {FamilyId}", current.FamilyId);
            await _refreshTokens.RevokeFamilyAsync(current.FamilyId, now, cancellationToken);
            return null;
        }

        var user = await _userManager.FindByIdAsync(current.UserId.ToString());
        if (user is null || (await _userManager.IsLockedOutAsync(user)))
        {
            await _refreshTokens.RevokeFamilyAsync(current.FamilyId, now, cancellationToken);
            return null;
        }

        var replacement = CreateRefreshToken(user.Id, current.FamilyId, now);
        var rotated = await _refreshTokens.RotateAsync(current, replacement.Entity, cancellationToken);
        if (!rotated)
        {
            await _refreshTokens.RevokeFamilyAsync(current.FamilyId, now, cancellationToken);
            return null;
        }

        return await CreateTokenPairAsync(user, replacement.RawToken, replacement.Entity.ExpiresAtUtc, now);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var token = await _refreshTokens.GetByHashAsync(HashToken(refreshToken), cancellationToken);
        if (token is null)
            return false;

        await _refreshTokens.RevokeFamilyAsync(token.FamilyId, DateTime.UtcNow, cancellationToken);
        return true;
    }

    private async Task<AuthTokenDto> CreateTokenPairAsync(
        AppUser user,
        string rawRefreshToken,
        DateTime refreshExpiresAtUtc,
        DateTime now)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            [ClaimTypes.Role] = roles.ToArray()
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims[JwtRegisteredClaimNames.Email] = user.Email;
        if (!string.IsNullOrWhiteSpace(user.UserName))
            claims[JwtRegisteredClaimNames.UniqueName] = user.UserName;

        var accessExpiresAtUtc = now.AddMinutes(_options.AccessTokenExpirationMinutes);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            IssuedAt = now,
            NotBefore = now,
            Expires = accessExpiresAtUtc,
            Claims = claims,
            SigningCredentials = _signingCredentials
        };

        return new AuthTokenDto(
            new JsonWebTokenHandler().CreateToken(descriptor),
            rawRefreshToken,
            accessExpiresAtUtc,
            refreshExpiresAtUtc);
    }

    private (RefreshToken Entity, string RawToken) CreateRefreshToken(Guid userId, Guid familyId, DateTime now)
    {
        var rawToken = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));
        return (new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FamilyId = familyId,
            TokenHash = HashToken(rawToken),
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddDays(_options.RefreshTokenExpirationDays)
        }, rawToken);
    }

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
