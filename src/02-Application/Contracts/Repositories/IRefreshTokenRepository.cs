using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> RotateAsync(RefreshToken current, RefreshToken replacement, CancellationToken cancellationToken = default);
    Task RevokeFamilyAsync(Guid familyId, DateTime revokedAtUtc, CancellationToken cancellationToken = default);
}
