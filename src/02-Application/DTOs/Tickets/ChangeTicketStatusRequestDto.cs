using Ticketing.Domain.Enums;

namespace Ticketing.Application.DTOs.Tickets;

public sealed record ChangeTicketStatusRequestDto(TicketLifeCycleEnum Status);
