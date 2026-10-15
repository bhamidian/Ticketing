using Ticketing.Domain.Enums;

namespace Ticketing.Application.DTOs.Tickets;

public sealed record TicketSummaryDto(
    Guid Id,
    string Subject,
    TicketLifeCycleEnum Status,
    TicketPriorityEnum Priority,
    Guid NormalUserId,
    string? NormalUserEmail,
    Guid? AssignedAgentId,
    string? AssignedAgentName,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
