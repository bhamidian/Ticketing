using FluentValidation;

namespace Ticketing.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator() =>
        RuleFor(command => command.RefreshToken).NotEmpty().MaximumLength(256);
}
