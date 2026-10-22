using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Commands.UpdateMessage;

public sealed class UpdateMessageCommandHandler(ITicketMessageRepository messages)
    : IRequestHandler<UpdateMessageCommand, Result<TicketMessageDto>>
{
    public async Task<Result<TicketMessageDto>> Handle(
        UpdateMessageCommand request,
        CancellationToken cancellationToken)
    {
        var message = await messages.GetByTicketIdAdminAsync(
            request.TicketId,
            request.MessageId,
            cancellationToken);
        if (message is null)
            return Result<TicketMessageDto>.Failure(ApplicationErrors.TicketMessageNotFound(request.MessageId));

        message.Message = request.Message.Trim();
        message.UpdatedAt = DateTime.UtcNow;
        message.UpdatedById = request.AdminUserId;
        await messages.SaveChangesAsync(cancellationToken);

        return Result<TicketMessageDto>.Success(ApplicationMapper.ToTicketMessageDto(message));
    }
}
