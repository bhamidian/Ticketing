using Ticketing.Application.DTOs.AgentDtos;
using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Repositories;

public interface IAgentRepository : IRepository<Agent>
{
    Task<Agent?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<List<GetReportAgentDto>> GetAgentResponseCountsAsync(CancellationToken cancellationToken = default);
    Task<List<GetAllAgentDto>> GetAllAgentsAsync(CancellationToken cancellationToken = default);
}
