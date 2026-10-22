using MediatR;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string Email, string Password) : IRequest<Result<AuthTokenDto>>;
