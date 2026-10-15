using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Results;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Commands.ChangeStatus;

public sealed class ChangeTicketStatusCommandHandler(ITicketRepository tickets)
    : IRequestHandler<ChangeTicketStatusCommand, Result<TicketDetailsDto>>
{
    public async Task<Result<TicketDetailsDto>> Handle(
        ChangeTicketStatusCommand request,
        CancellationToken cancellationToken)
    {
        if (!request.IsStaff)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.Forbidden("فقط پشتیبان می‌تواند وضعیت تیکت را تغییر دهد."));

        var ticket = await tickets.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(request.TicketId));

        if (ticket.AssignedAgentId != request.UserId)
            return Result<TicketDetailsDto>.Failure(ticket.AssignedAgentId is null
                ? ApplicationErrors.TicketMustBeClaimed
                : ApplicationErrors.TicketAlreadyAssigned);

        var now = DateTime.UtcNow;
        ticket.TicketLifeCycle = request.Status;
        ticket.ClosedAt = request.Status == TicketLifeCycleEnum.CLOSED ? ticket.ClosedAt ?? now : null;
        ticket.ClosedById = request.Status == TicketLifeCycleEnum.CLOSED ? ticket.ClosedById ?? request.UserId : null;
        ticket.UpdatedAt = now;
        ticket.UpdatedById = request.UserId;

        await tickets.SaveChangesAsync(cancellationToken);

        var updatedTicket = await tickets.GetWithMessagesAsync(ticket.Id, cancellationToken);
        if (updatedTicket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(ticket.Id));

        return Result<TicketDetailsDto>.Success(ApplicationMapper.ToTicketDetailsDto(updatedTicket));
    }
}
