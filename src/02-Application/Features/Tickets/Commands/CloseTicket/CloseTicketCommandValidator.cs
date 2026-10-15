using FluentValidation;

namespace Ticketing.Application.Features.Tickets.Commands.CloseTicket;

public sealed class CloseTicketCommandValidator : AbstractValidator<CloseTicketCommand>
{
    public CloseTicketCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.TicketId).NotEmpty();
    }
}
