using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Features.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Results;
using Ticketing.Domain.Entites;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Commands.AddMessage;

public sealed class AddTicketMessageCommandHandler(
    ITicketRepository tickets,
    ITicketMessageRepository messages,
    INormalUserRepository normalUsers) : IRequestHandler<AddTicketMessageCommand, Result<TicketDetailsDto>>
{
    public async Task<Result<TicketDetailsDto>> Handle(AddTicketMessageCommand request, CancellationToken cancellationToken)
    {
        var ticket = await tickets.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(request.TicketId));

        var accessError = await TicketAccess.GetAccessErrorAsync(
            ticket, request.UserId, request.IsStaff, normalUsers, cancellationToken);
        if (accessError is not null)
            return Result<TicketDetailsDto>.Failure(accessError);

        if (ticket.TicketLifeCycle == TicketLifeCycleEnum.CLOSED)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketClosed);

        if (request.IsStaff && ticket.AssignedAgentId != request.UserId)
            return Result<TicketDetailsDto>.Failure(ticket.AssignedAgentId is null
                ? ApplicationErrors.TicketMustBeClaimed
                : ApplicationErrors.TicketAlreadyAssigned);

        var text = request.Message.Trim();

        var now = DateTime.UtcNow;
        var message = new TicketMessage
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            SenderId = request.UserId,
            Message = text,
            PictureUrl = request.PictureUrl?.Trim(),
            CreatedAt = now,
            UpdatedAt = now,
            CreatedById = request.UserId,
            UpdatedById = request.UserId
        };

        ticket.UpdatedAt = now;
        ticket.UpdatedById = request.UserId;
        if (request.IsStaff)
            ticket.TicketLifeCycle = TicketLifeCycleEnum.ANSWERED;
        else if (ticket.TicketLifeCycle == TicketLifeCycleEnum.ANSWERED)
            ticket.TicketLifeCycle = TicketLifeCycleEnum.INPROGRESS;

        await messages.AddAsync(message, cancellationToken);
        await tickets.SaveChangesAsync(cancellationToken);

        var updatedTicket = await tickets.GetWithMessagesAsync(ticket.Id, cancellationToken)
            ?? null;
        if (updatedTicket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(ticket.Id));

        return Result<TicketDetailsDto>.Success(ApplicationMapper.ToTicketDetailsDto(updatedTicket));
    }
}
