using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Commands.ClaimTicket;

public sealed record ClaimTicketCommand(Guid UserId, bool IsStaff, Guid TicketId)
    : IRequest<Result<TicketDetailsDto>>;
