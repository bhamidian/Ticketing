namespace Ticketing.Application.DTOs.Tickets;

public sealed record AddTicketMessageRequestDto(string Message, string? PictureUrl = null);
