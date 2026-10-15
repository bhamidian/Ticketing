using Ticketing.Domain.Entites;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Contracts.Repositories;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<Ticket?> GetWithMessagesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Ticket> Items, int TotalCount)> GetFilteredAsync(
        Guid? normalUserId,
        TicketLifeCycleEnum? status,
        TicketPriorityEnum? priority,
        Guid? visibleToAgentId,
        bool assignedOnly,
        bool unassignedOnly,
        int page,
        int pageSize,
        bool oldestFirst,
        CancellationToken cancellationToken = default);
    Task<bool> TryAssignToAgentAsync(Guid ticketId, Guid agentUserId, DateTime assignedAtUtc, CancellationToken cancellationToken = default);
}
