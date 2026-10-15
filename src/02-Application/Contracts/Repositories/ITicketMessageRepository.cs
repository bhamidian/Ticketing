using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Repositories;

public interface ITicketMessageRepository : IRepository<TicketMessage>
{
    Task<IReadOnlyList<TicketMessage>> GetByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
}
