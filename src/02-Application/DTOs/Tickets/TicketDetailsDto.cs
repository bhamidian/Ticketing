using Ticketing.Domain.Enums;

namespace Ticketing.Application.DTOs.Tickets;

public sealed record TicketDetailsDto(
    Guid Id,
    string Subject,
    TicketLifeCycleEnum Status,
    TicketPriorityEnum Priority,
    Guid NormalUserId,
    string? NormalUserEmail,
    Guid? AssignedAgentId,
    string? AssignedAgentName,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    DateTime? ClosedAtUtc,
    Guid? ClosedById,
    IReadOnlyList<TicketMessageDto> Messages,
    string? DepartmentName);
