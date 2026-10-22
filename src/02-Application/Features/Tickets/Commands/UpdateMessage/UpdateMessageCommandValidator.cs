using FluentValidation;

namespace Ticketing.Application.Features.Tickets.Commands.UpdateMessage
{
    public class UpdateMessageCommandValidator : AbstractValidator<UpdateMessageCommand>
    {
        public UpdateMessageCommandValidator()
        {
            RuleFor(command => command.AdminUserId).NotEmpty();
            RuleFor(command => command.TicketId).NotEmpty();
            RuleFor(command => command.MessageId).NotEmpty();
            RuleFor(command => command.Message)
                .Must(message => !string.IsNullOrWhiteSpace(message))
                .WithMessage("متن پیام نمی‌تواند خالی باشد.")
                .MaximumLength(4000);
        }
    }
}
