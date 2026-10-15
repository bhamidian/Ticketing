using MediatR;
using Ticketing.Application.Contracts.Services;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandler(IJwtService jwtService) : IRequestHandler<LogoutCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!await jwtService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken))
            return Result<bool>.Failure(ApplicationErrors.InvalidCredentials);

        return Result<bool>.Success(true);
    }
}
