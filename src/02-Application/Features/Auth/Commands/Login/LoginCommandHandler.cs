using MediatR;
using Ticketing.Application.Contracts.Services;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserCredentialService credentials,
    IJwtService jwtService) : IRequestHandler<LoginCommand, Result<AuthTokenDto>>
{
    public async Task<Result<AuthTokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await credentials.ValidateCredentialsAsync(
            request.EmailOrUserName,
            request.Password,
            cancellationToken);

        if (user is null)
            return Result<AuthTokenDto>.Failure(ApplicationErrors.InvalidCredentials);

        var tokens = await jwtService.CreateTokenPairAsync(user, cancellationToken);
        return Result<AuthTokenDto>.Success(tokens);
    }
}
