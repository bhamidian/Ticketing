using MediatR;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthTokenDto>>;
