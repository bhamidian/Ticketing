using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ticketing.Domain.Entites;
using Ticketing.Application.DTOs.Auth;
using Ticketing.Application.Contracts.Services;
using Ticketing.Application.Features.Auth.Commands.RefreshToken;
using Ticketing.Application.Mapping;
using Ticketing.Endpoint.Webapi.Mapping;
using Ticketing.Endpoint.Webapi.DTOs;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Endpoint.Webapi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public sealed class AuthController(
    ISender sender,
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext,
    IJwtService jwtService) : ControllerBase
{
    private const string RefreshCookieName = "ticketing.refresh";

    [HttpPost("register")]
    [ProducesResponseType<AuthTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        if (await userManager.FindByEmailAsync(email) is not null)
            return BadRequest(new ProblemDetails { Title = "ثبت‌نام انجام نشد", Detail = "این ایمیل قبلاً ثبت شده است." });

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var user = new AppUser { Id = Guid.NewGuid(), UserName = email, Email = email };
        var created = await userManager.CreateAsync(user, request.Password);
        if (!created.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            var message = string.Join(" ", created.Errors.Select(error => error.Code switch
            {
                "PasswordTooShort" => "گذرواژه باید حداقل ۸ نویسه باشد.",
                "PasswordRequiresDigit" => "گذرواژه باید دست‌کم یک عدد داشته باشد.",
                "PasswordRequiresUpper" => "گذرواژه باید دست‌کم یک حرف بزرگ انگلیسی داشته باشد.",
                "PasswordRequiresLower" => "گذرواژه باید دست‌کم یک حرف کوچک انگلیسی داشته باشد.",
                "PasswordRequiresNonAlphanumeric" => "گذرواژه باید دست‌کم یک نماد داشته باشد.",
                _ => "اطلاعات واردشده معتبر نیست."
            }).Distinct());
            return BadRequest(new ProblemDetails { Title = "ثبت‌نام انجام نشد", Detail = message });
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "ساخت حساب انجام نشد.");
        }

        var now = DateTime.UtcNow;
        dbContext.NormalUsers.Add(new NormalUser
        {
            Id = Guid.NewGuid(), AppUserId = user.Id,
            CreatedAt = now, UpdatedAt = now,
            CreatedById = user.Id, UpdatedById = user.Id
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        var tokens = await jwtService.CreateTokenPairAsync(user, cancellationToken);
        SetRefreshCookie(tokens);
        return Ok(tokens);
    }

    [HttpPost("login")]
    [ProducesResponseType<AuthTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(ApplicationMapper.ToCommand(request), cancellationToken);
        if (!result.IsSuccess)
            return ApiResultMapper.ToProblem(result.Error!);

        SetRefreshCookie(result.Value!);
        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    [ProducesResponseType<AuthTokenDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(RefreshCookieName, out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
            return Unauthorized();

        var result = await sender.Send(new RefreshTokenCommand(refreshToken), cancellationToken);
        if (!result.IsSuccess)
        {
            ClearRefreshCookie();
            return ApiResultMapper.ToProblem(result.Error!);
        }

        SetRefreshCookie(result.Value!);
        return Ok(result.Value);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue(RefreshCookieName, out var refreshToken) && !string.IsNullOrWhiteSpace(refreshToken))
        {
            var result = await sender.Send(ApplicationMapper.ToLogoutCommand(new RefreshTokenRequestDto(refreshToken)), cancellationToken);
            if (!result.IsSuccess)
            {
                ClearRefreshCookie();
                return ApiResultMapper.ToProblem(result.Error!);
            }
        }

        ClearRefreshCookie();
        return NoContent();
    }

    private void SetRefreshCookie(AuthTokenDto tokens) => Response.Cookies.Append(RefreshCookieName, tokens.RefreshToken, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/api/Auth",
        Expires = new DateTimeOffset(DateTime.SpecifyKind(tokens.RefreshTokenExpiresAtUtc, DateTimeKind.Utc))
    });

    private void ClearRefreshCookie() => Response.Cookies.Delete(RefreshCookieName, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/api/Auth"
    });
}
