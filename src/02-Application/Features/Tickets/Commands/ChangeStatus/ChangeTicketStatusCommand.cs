using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Commands.ChangeStatus;

public sealed record ChangeTicketStatusCommand(
    Guid UserId,
    bool IsStaff,
    Guid TicketId,
    TicketLifeCycleEnum Status) : IRequest<Result<TicketDetailsDto>>;
