using Ticketing.Domain.Enums;

namespace Ticketing.Application.DTOs.Tickets;

public sealed record CreateTicketRequestDto(string Subject, string Message, TicketPriorityEnum Priority, string? PictureUrl = null);
