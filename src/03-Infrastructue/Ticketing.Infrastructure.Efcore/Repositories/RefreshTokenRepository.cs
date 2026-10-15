using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Repositories;

public sealed class RefreshTokenRepository(
    ApplicationDbContext dbContext,
    ILogger<RefreshTokenRepository> logger) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default) =>
        dbContext.RefreshTokens.SingleOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default) =>
        await dbContext.RefreshTokens.AddAsync(token, cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);

    public async Task<bool> RotateAsync(
        RefreshToken current,
        RefreshToken replacement,
        CancellationToken cancellationToken = default)
    {
        current.RevokedAtUtc = replacement.CreatedAtUtc;
        current.ReplacedByTokenHash = replacement.TokenHash;
        await dbContext.RefreshTokens.AddAsync(replacement, cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            logger.LogWarning(exception, "Refresh token rotation lost a concurrency race for token family {FamilyId}", current.FamilyId);
            dbContext.Entry(replacement).State = EntityState.Detached;
            dbContext.Entry(current).State = EntityState.Detached;
            return false;
        }
    }

    public async Task RevokeFamilyAsync(Guid familyId, DateTime revokedAtUtc, CancellationToken cancellationToken = default)
    {
        var activeTokens = await dbContext.RefreshTokens
            .Where(token => token.FamilyId == familyId && token.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
            token.RevokedAtUtc = revokedAtUtc;

        if (activeTokens.Count > 0)
            await dbContext.SaveChangesAsync(cancellationToken);
    }
}
