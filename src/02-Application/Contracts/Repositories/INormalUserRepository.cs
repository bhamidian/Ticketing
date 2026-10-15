using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Repositories;

public interface INormalUserRepository : IRepository<NormalUser>
{
    Task<NormalUser?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
}
