using MediatR;
using Ticketing.Application.Results;
using Ticketing.Application.DTOs.Tickets;

namespace Ticketing.Application.Features.Tickets.Commands.UpdateMessage
{
    public sealed record UpdateMessageCommand(
        Guid AdminUserId,
        Guid TicketId,
        Guid MessageId,
        string Message) : IRequest<Result<TicketMessageDto>>;
}
