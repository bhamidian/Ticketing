using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Ticketing.Application.Exceptions;

namespace Ticketing.Endpoint.Webapi.Middleware;

public sealed class ApplicationExceptionMiddleware(
    RequestDelegate next,
    ILogger<ApplicationExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception) when (!context.Response.HasStarted)
        {
            var (statusCode, title) = exception switch
            {
                FluentValidation.ValidationException => (StatusCodes.Status400BadRequest, "اعتبارسنجی اطلاعات ناموفق بود"),
                AuthenticationFailedException => (StatusCodes.Status401Unauthorized, "احراز هویت انجام نشد"),
                ForbiddenAccessException => (StatusCodes.Status403Forbidden, "دسترسی غیرمجاز"),
                NotFoundException => (StatusCodes.Status404NotFound, "مورد درخواستی پیدا نشد"),
                _ => (StatusCodes.Status500InternalServerError, "خطای غیرمنتظره‌ای رخ داد")
            };

            if (statusCode >= 500)
                logger.LogError(exception, "Unhandled application error for {Path}", context.Request.Path);

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = statusCode switch
                {
                    StatusCodes.Status400BadRequest => "لطفاً اطلاعات واردشده را بررسی کنید.",
                    StatusCodes.Status401Unauthorized => "برای ادامه دوباره وارد حساب کاربری شوید.",
                    StatusCodes.Status403Forbidden => "اجازه انجام این عملیات را ندارید.",
                    StatusCodes.Status404NotFound => "مورد درخواستی پیدا نشد.",
                    _ => null
                },
                Instance = context.Request.Path
            }, context.RequestAborted);
        }
    }
}
