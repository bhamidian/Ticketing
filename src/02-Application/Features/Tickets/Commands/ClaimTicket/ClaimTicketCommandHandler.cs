using MediatR;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Commands.ClaimTicket;

public sealed class ClaimTicketCommandHandler : IRequestHandler<ClaimTicketCommand, Result<TicketDetailsDto>>
{
    public async Task<Result<TicketDetailsDto>> Handle(
        ClaimTicketCommand request,
        CancellationToken cancellationToken)
    {
        return Result<TicketDetailsDto>.Failure(
            ApplicationErrors.Forbidden("تخصیص تیکت فقط توسط ادمین انجام می‌شود."));
    }
}
