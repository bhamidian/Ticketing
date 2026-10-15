using MediatR;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string EmailOrUserName, string Password) : IRequest<Result<AuthTokenDto>>;
