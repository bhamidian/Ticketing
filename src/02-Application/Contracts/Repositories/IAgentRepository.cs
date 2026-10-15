using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Repositories;

public interface IAgentRepository : IRepository<Agent>
{
    Task<Agent?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
}
