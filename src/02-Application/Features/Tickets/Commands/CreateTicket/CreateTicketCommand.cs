using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Commands.CreateTicket;

public sealed record CreateTicketCommand(Guid UserId, string Subject, string Message, TicketPriorityEnum Priority, string? PictureUrl = null)
    : IRequest<Result<TicketDetailsDto>>;
