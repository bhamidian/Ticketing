using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.Tickets;
using Ticketing.Application.Mapping;
using Ticketing.Application.Results;
using Ticketing.Domain.Entites;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.Features.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler(
    ITicketRepository tickets,
    INormalUserRepository normalUsers) : IRequestHandler<CreateTicketCommand, Result<TicketDetailsDto>>
{
    public async Task<Result<TicketDetailsDto>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var profile = await normalUsers.GetByAppUserIdAsync(request.UserId, cancellationToken);
        if (profile is null)
            return Result<TicketDetailsDto>.Failure(ApplicationErrors.Forbidden("فقط کاربر عادی می‌تواند تیکت ثبت کند."));

        var subject = request.Subject.Trim();

        var now = DateTime.UtcNow;
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Subject = subject,
            TicketLifeCycle = TicketLifeCycleEnum.OPEN,
            TicketPriority = request.Priority,
            NormalUserId = profile.Id,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedById = request.UserId,
            UpdatedById = request.UserId
        };

        var initialMessage = new TicketMessage
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            Ticket = ticket,
            SenderId = request.UserId,
            Message = request.Message.Trim(),
            PictureUrl = request.PictureUrl,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedById = request.UserId,
            UpdatedById = request.UserId
        };
        ticket.TicketMessages.Add(initialMessage);

        await tickets.AddAsync(ticket, cancellationToken);
        await tickets.SaveChangesAsync(cancellationToken);
        return Result<TicketDetailsDto>.Success(ApplicationMapper.ToTicketDetailsDto(ticket));
    }
}
