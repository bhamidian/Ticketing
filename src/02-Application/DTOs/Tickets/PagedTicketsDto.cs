namespace Ticketing.Application.DTOs.Tickets;

public sealed record PagedTicketsDto(
    IReadOnlyList<TicketSummaryDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
