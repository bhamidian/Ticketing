using MediatR;
using Ticketing.Application.Contracts.Services;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(IJwtService jwtService)
    : IRequestHandler<RefreshTokenCommand, Result<AuthTokenDto>>
{
    public async Task<Result<AuthTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenPair = await jwtService.RefreshTokenPairAsync(request.RefreshToken, cancellationToken);
        return tokenPair is null
            ? Result<AuthTokenDto>.Failure(ApplicationErrors.InvalidCredentials)
            : Result<AuthTokenDto>.Success(tokenPair);
    }
}
