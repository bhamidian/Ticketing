using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Features.Tickets;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Queries.GetTicketImage;

public sealed class GetTicketImageQueryHandler(
    ITicketRepository tickets,
    INormalUserRepository normalUsers) : IRequestHandler<GetTicketImageQuery, Result<TicketImageDto>>
{
    public async Task<Result<TicketImageDto>> Handle(GetTicketImageQuery request, CancellationToken cancellationToken)
    {
        var ticket = await tickets.GetWithMessagesAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return Result<TicketImageDto>.Failure(ApplicationErrors.TicketNotFound(request.TicketId));

        var accessError = await TicketAccess.GetAccessErrorAsync(
            ticket, request.UserId, request.IsStaff, normalUsers, cancellationToken, request.IsAdmin);
        if (accessError is not null)
            return Result<TicketImageDto>.Failure(accessError);

        var imagePath = ticket.TicketMessages
            .SingleOrDefault(message => message.Id == request.MessageId &&
                (request.IsStaff || request.IsAdmin || message.IsConfirmed == true))?.PictureUrl;
        if (string.IsNullOrWhiteSpace(imagePath) ||
            !imagePath.StartsWith("/Teams/Tickets/", StringComparison.Ordinal))
            return Result<TicketImageDto>.Failure(
                new ApplicationError("ticket.image_not_found", "تصویر پیوست‌شده پیدا نشد.", ApplicationErrorType.NotFound));

        var extension = Path.GetExtension(imagePath).ToLowerInvariant();
        if (extension is not (".jpg" or ".jpeg" or ".png" or ".gif" or ".webp"))
            return Result<TicketImageDto>.Failure(
                new ApplicationError("ticket.image_not_found", "تصویر پیوست‌شده پیدا نشد.", ApplicationErrorType.NotFound));

        return Result<TicketImageDto>.Success(new TicketImageDto(imagePath));
    }
}
