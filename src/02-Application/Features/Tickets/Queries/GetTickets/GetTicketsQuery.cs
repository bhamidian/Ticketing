using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Queries.GetTickets;

public sealed record GetTicketsQuery(
    Guid UserId,
    bool IsStaff,
    TicketLifeCycleEnum? Status = null,
    TicketPriorityEnum? Priority = null,
    int Page = 1,
    int PageSize = 12,
    bool OldestFirst = false,
    bool AssignedOnly = false,
    bool UnassignedOnly = false) : IRequest<Result<PagedTicketsDto>>;
