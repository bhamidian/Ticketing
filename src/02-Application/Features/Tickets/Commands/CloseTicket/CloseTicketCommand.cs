using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Commands.CloseTicket;

public sealed record CloseTicketCommand(Guid UserId, bool IsStaff, Guid TicketId) : IRequest<Result<TicketDetailsDto>>;
