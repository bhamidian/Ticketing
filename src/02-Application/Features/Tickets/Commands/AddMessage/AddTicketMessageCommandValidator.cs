using FluentValidation;

namespace Ticketing.Application.Features.Tickets.Commands.AddMessage;

public sealed class AddTicketMessageCommandValidator : AbstractValidator<AddTicketMessageCommand>
{
    public AddTicketMessageCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.TicketId).NotEmpty();
        RuleFor(command => command.Message).NotEmpty().MaximumLength(4000);
        RuleFor(command => command.PictureUrl).MaximumLength(2048);
    }
}
