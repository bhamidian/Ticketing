using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Results;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Commands.ClaimTicket;

public sealed class ClaimTicketCommandHandler(ITicketRepository tickets)
    : IRequestHandler<ClaimTicketCommand, Result<TicketDetailsDto>>
{
    public async Task<Result<TicketDetailsDto>> Handle(
        ClaimTicketCommand request,
        CancellationToken cancellationToken)
    {
        if (!request.IsStaff)
            return Result<TicketDetailsDto>.Failure(
                ApplicationErrors.Forbidden("فقط پشتیبان می‌تواند تیکت را بپذیرد."));

        var ticket = await tickets.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(request.TicketId));

        if (ticket.TicketLifeCycle == TicketLifeCycleEnum.CLOSED)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.ClosedTicketCannotBeAssigned);

        if (ticket.AssignedAgentId is not null && ticket.AssignedAgentId != request.UserId)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketAlreadyAssigned);

        var claimed = await tickets.TryAssignToAgentAsync(
            ticket.Id, request.UserId, DateTime.UtcNow, cancellationToken);
        if (!claimed)
        {
            var current = await tickets.GetByIdAsync(ticket.Id, cancellationToken);
            if (current is null)
                return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(ticket.Id));
            if (current.TicketLifeCycle == TicketLifeCycleEnum.CLOSED)
                return Result<TicketDetailsDto>.Failure(ApplicationErrors.ClosedTicketCannotBeAssigned);
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketAlreadyAssigned);
        }

        var updatedTicket = await tickets.GetWithMessagesAsync(ticket.Id, cancellationToken);
        if (updatedTicket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(ticket.Id));

        return Result<TicketDetailsDto>.Success(ApplicationMapper.ToTicketDetailsDto(updatedTicket));
    }
}
