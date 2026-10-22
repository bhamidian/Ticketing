using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Queries.GetTicketImage;

public sealed record GetTicketImageQuery(Guid UserId, bool IsStaff, Guid TicketId, Guid MessageId, bool IsAdmin = false)
    : IRequest<Result<TicketImageDto>>;
