using FluentValidation;

namespace Ticketing.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.EmailOrUserName)
            .NotEmpty()
            .MaximumLength(256);
        RuleFor(command => command.Password)
            .NotEmpty()
            .MaximumLength(256);
    }
}
