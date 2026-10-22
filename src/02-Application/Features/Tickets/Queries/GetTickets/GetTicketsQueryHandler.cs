using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Queries.GetTickets;

public sealed class GetTicketsQueryHandler(
    ITicketRepository tickets,
    INormalUserRepository normalUsers) : IRequestHandler<GetTicketsQuery, Result<PagedTicketsDto>>
{
    public async Task<Result<PagedTicketsDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        Guid? normalUserId = null;
        if (!request.IsStaff && !request.IsAdmin)
        {
            var profile = await normalUsers.GetByAppUserIdAsync(request.UserId, cancellationToken);
            if (profile is null)
                return Result<PagedTicketsDto>.Failure(
                    ApplicationErrors.Forbidden("برای مشاهده تیکت‌ها باید حساب کاربری عادی داشته باشید."));
            normalUserId = profile.Id;
        }

        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var page = Math.Clamp(request.Page, 1, int.MaxValue / pageSize);
        var (items, totalCount) = await tickets.GetFilteredAsync(
            normalUserId, request.Status, request.Priority,
            request.IsAdmin ? null : request.IsStaff ? request.UserId : null,
            request.IsStaff || (request.IsAdmin && request.AssignedOnly),
            request.IsAdmin && request.UnassignedOnly,
            page, pageSize, request.OldestFirst, cancellationToken);
        var mapped = items
            .Select(ApplicationMapper.ToTicketSummaryDto)
            .ToArray();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return Result<PagedTicketsDto>.Success(new PagedTicketsDto(mapped, page, pageSize, totalCount, totalPages));
    }
}
