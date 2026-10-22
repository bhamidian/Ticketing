using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Queries.GetTicketDetails;

public sealed record GetTicketDetailsQuery(Guid UserId, bool IsStaff, Guid TicketId, bool IsAdmin = false)
    : IRequest<Result<TicketDetailsDto>>;
