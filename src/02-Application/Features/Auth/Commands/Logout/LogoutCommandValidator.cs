using FluentValidation;

namespace Ticketing.Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator() =>
        RuleFor(command => command.RefreshToken).NotEmpty().MaximumLength(256);
}
