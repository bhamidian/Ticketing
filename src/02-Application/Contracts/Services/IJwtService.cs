using Ticketing.Application.DTOs.Auth;
using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Services;

public interface IJwtService
{
    Task<AuthTokenDto> CreateTokenPairAsync(AppUser user, CancellationToken cancellationToken = default);
    Task<AuthTokenDto?> RefreshTokenPairAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
