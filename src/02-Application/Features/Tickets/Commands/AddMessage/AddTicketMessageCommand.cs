using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Commands.AddMessage;

public sealed record AddTicketMessageCommand(
    Guid UserId,
    bool IsStaff,
    Guid TicketId,
    string Message,
    string? PictureUrl) : IRequest<Result<TicketDetailsDto>>;
