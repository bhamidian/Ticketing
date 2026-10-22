namespace Ticketing.Application.DTOs.Tickets;

public sealed record TicketMessageDto(
    Guid Id,
    Guid SenderId,
    string SenderName,
    string Message,
    string? PictureUrl,
    DateTime CreatedAtUtc,
    bool? IsConfirmed);
