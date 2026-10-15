using MediatR;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest<Result<bool>>;
