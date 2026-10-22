using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Repositories;

public interface ITicketMessageRepository : IRepository<TicketMessage>
{
    Task<List<TicketMessage>> GetByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task<TicketMessage?> GetByTicketIdAdminAsync(
        Guid ticketId,
        Guid messageId,
        CancellationToken cancellationToken = default);

    Task<bool> ConfirmAgentMessageAsync(
        Guid messageId,
        Guid adminUserId,
        CancellationToken cancellationToken = default
    );

}
