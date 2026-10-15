using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Features.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Results;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Commands.CloseTicket;

public sealed class CloseTicketCommandHandler(
    ITicketRepository tickets) : IRequestHandler<CloseTicketCommand, Result<TicketDetailsDto>>
{
    public async Task<Result<TicketDetailsDto>> Handle(CloseTicketCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsStaff)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.Forbidden("فقط پشتیبان می‌تواند تیکت را ببندد."));

        var ticket = await tickets.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(request.TicketId));

        if (ticket.TicketLifeCycle == TicketLifeCycleEnum.CLOSED)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketAlreadyClosed);

        if (ticket.AssignedAgentId != request.UserId)
            return Result<TicketDetailsDto>.Failure(ticket.AssignedAgentId is null
                ? ApplicationErrors.TicketMustBeClaimed
                : ApplicationErrors.TicketAlreadyAssigned);

        var now = DateTime.UtcNow;
        ticket.TicketLifeCycle = TicketLifeCycleEnum.CLOSED;
        ticket.ClosedAt = now;
        ticket.ClosedById = request.UserId;
        ticket.UpdatedAt = now;
        ticket.UpdatedById = request.UserId;
        await tickets.SaveChangesAsync(cancellationToken);

        var closedTicket = await tickets.GetWithMessagesAsync(ticket.Id, cancellationToken)
            ?? null;
        if (closedTicket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(ticket.Id));

        return Result<TicketDetailsDto>.Success(ApplicationMapper.ToTicketDetailsDto(closedTicket));
    }
}
