using FluentValidation;

namespace Ticketing.Application.Features.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Subject).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Message).NotEmpty().MaximumLength(4000);
        RuleFor(command => command.Priority).IsInEnum();
        RuleFor(command => command.PictureUrl).MaximumLength(2048);
    }
}
