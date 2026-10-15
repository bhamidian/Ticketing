using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Features.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Queries.GetTicketDetails;

public sealed class GetTicketDetailsQueryHandler(
    ITicketRepository tickets,
    INormalUserRepository normalUsers) : IRequestHandler<GetTicketDetailsQuery, Result<TicketDetailsDto>>
{
    public async Task<Result<TicketDetailsDto>> Handle(GetTicketDetailsQuery request, CancellationToken cancellationToken)
    {
        var ticket = await tickets.GetWithMessagesAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.TicketNotFound(request.TicketId));

        var accessError = await TicketAccess.GetAccessErrorAsync(
            ticket, request.UserId, request.IsStaff, normalUsers, cancellationToken);
        if (accessError is not null)
            return Result<TicketDetailsDto>.Failure(accessError);

        return Result<TicketDetailsDto>.Success(ApplicationMapper.ToTicketDetailsDto(ticket));
    }
}
