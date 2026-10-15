using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Features.Auth.Commands.Login;
using Ticketing.Application.Features.Auth.Commands.Logout;
using Ticketing.Application.Features.Auth.Commands.RefreshToken;
using Ticketing.Application.Features.Tickets.Commands.AddMessage;
using Ticketing.Application.Features.Tickets.Commands.CloseTicket;
using Ticketing.Application.Features.Tickets.Commands.ChangeStatus;
using Ticketing.Application.Features.Tickets.Commands.ClaimTicket;
using Ticketing.Application.Features.Tickets.Commands.CreateTicket;
using Ticketing.Application.Features.Tickets.Queries.GetTicketDetails;
using Ticketing.Application.Features.Tickets.Queries.GetTickets;
using Ticketing.Domain.Entites;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Mapping;

public static class ApplicationMapper
{
    public static LoginCommand ToCommand(LoginRequestDto dto) => new(dto.EmailOrUserName, dto.Password);
    public static RefreshTokenCommand ToCommand(RefreshTokenRequestDto dto) => new(dto.RefreshToken);
    public static LogoutCommand ToLogoutCommand(RefreshTokenRequestDto dto) => new(dto.RefreshToken);
    public static CreateTicketCommand ToCommand(CreateTicketRequestDto dto, Guid userId) =>
        new(userId, dto.Subject, dto.Message, dto.Priority, dto.PictureUrl);
    public static AddTicketMessageCommand ToCommand(AddTicketMessageRequestDto dto, Guid userId, bool isStaff, Guid ticketId) =>
        new(userId, isStaff, ticketId, dto.Message, dto.PictureUrl);
    public static CloseTicketCommand ToCloseTicketCommand(Guid userId, bool isStaff, Guid ticketId) =>
        new(userId, isStaff, ticketId);
    public static ChangeTicketStatusCommand ToChangeTicketStatusCommand(
        Guid userId,
        bool isStaff,
        Guid ticketId,
        TicketLifeCycleEnum status) => new(userId, isStaff, ticketId, status);
    public static ClaimTicketCommand ToClaimTicketCommand(Guid userId, bool isStaff, Guid ticketId) =>
        new(userId, isStaff, ticketId);
    public static GetTicketsQuery ToGetTicketsQuery(
        Guid userId,
        bool isStaff,
        TicketLifeCycleEnum? status = null,
        TicketPriorityEnum? priority = null,
        int page = 1,
        int pageSize = 12,
        bool oldestFirst = false,
        bool assignedOnly = false,
        bool unassignedOnly = false) =>
        new(userId, isStaff, status, priority, page, pageSize, oldestFirst, assignedOnly, unassignedOnly);
    public static GetTicketDetailsQuery ToGetTicketDetailsQuery(Guid userId, bool isStaff, Guid ticketId) =>
        new(userId, isStaff, ticketId);

    public static TicketSummaryDto ToTicketSummaryDto(Ticket ticket) => new(
        ticket.Id,
        ticket.Subject,
        ticket.TicketLifeCycle,
        ticket.TicketPriority,
        ticket.NormalUserId,
        ticket.NormalUser?.AppUser?.Email,
        ticket.AssignedAgentId,
        ticket.AssignedAgent?.UserName ?? ticket.AssignedAgent?.Email,
        ticket.CreatedAt,
        ticket.UpdatedAt);

    public static TicketDetailsDto ToTicketDetailsDto(Ticket ticket) => new(
        ticket.Id,
        ticket.Subject,
        ticket.TicketLifeCycle,
        ticket.TicketPriority,
        ticket.NormalUserId,
        ticket.NormalUser?.AppUser?.Email,
        ticket.AssignedAgentId,
        ticket.AssignedAgent?.UserName ?? ticket.AssignedAgent?.Email,
        ticket.CreatedAt,
        ticket.UpdatedAt,
        ticket.ClosedAt,
        ticket.ClosedById,
        ticket.TicketMessages
            .OrderBy(message => message.CreatedAt)
            .Select(ToTicketMessageDto)
            .ToArray());

    public static TicketMessageDto ToTicketMessageDto(TicketMessage message) => new(
        message.Id,
        message.SenderId,
        message.Sender?.UserName ?? message.Sender?.Email ?? message.SenderId.ToString(),
        message.Message,
        string.IsNullOrWhiteSpace(message.PictureUrl)
            ? null
            : $"/api/Ticket/{message.TicketId}/messages/{message.Id}/image",
        message.CreatedAt);
}
