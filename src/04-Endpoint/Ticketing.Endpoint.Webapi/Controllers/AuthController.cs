using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Features.Auth.Commands.RefreshToken;
using Ticketing.Application.Features.Auth.Commands.Register;
using Ticketing.Application.Mapping;
using Ticketing.Endpoint.Webapi.Mapping;
using Ticketing.Endpoint.Webapi.DTOs;
using Ticketing.Endpoint.Webapi.Cookies;

namespace Ticketing.Endpoint.Webapi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public sealed class AuthController(
    ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType<AuthTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);
        if (!result.IsSuccess)
            return ApiResultMapper.ToProblem(result.Error!);

        RefreshTokenCookie.Set(Response, result.Value!);
        return Ok(result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType<AuthTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(ApplicationMapper.ToCommand(request), cancellationToken);
        if (!result.IsSuccess)
            return ApiResultMapper.ToProblem(result.Error!);

        RefreshTokenCookie.Set(Response, result.Value!);
        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    [ProducesResponseType<AuthTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!RefreshTokenCookie.TryRead(Request, out var refreshToken))
            return Unauthorized();

        var result = await sender.Send(new RefreshTokenCommand(refreshToken), cancellationToken);
        if (!result.IsSuccess)
        {
            RefreshTokenCookie.Clear(Response);
            return ApiResultMapper.ToProblem(result.Error!);
        }

        RefreshTokenCookie.Set(Response, result.Value!);
        return Ok(result.Value);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (RefreshTokenCookie.TryRead(Request, out var refreshToken))
        {
            var result = await sender.Send(ApplicationMapper.ToLogoutCommand(new RefreshTokenRequestDto(refreshToken)), cancellationToken);
            if (!result.IsSuccess)
            {
                RefreshTokenCookie.Clear(Response);
                return ApiResultMapper.ToProblem(result.Error!);
            }
        }

        RefreshTokenCookie.Clear(Response);
        return NoContent();
    }
}
