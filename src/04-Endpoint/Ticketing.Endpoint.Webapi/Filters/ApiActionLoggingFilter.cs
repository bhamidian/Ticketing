using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Ticketing.Endpoint.Webapi.Filters;

public sealed class ApiActionLoggingFilter(ILogger<ApiActionLoggingFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        var executedContext = await next();

        // Unhandled exceptions are logged by ApplicationExceptionMiddleware.
        if (executedContext.Exception is not null && !executedContext.ExceptionHandled)
            return;

        stopwatch.Stop();
        var statusCode = (executedContext.Result as IStatusCodeActionResult)?.StatusCode
            ?? context.HttpContext.Response.StatusCode;
        var actionName = context.ActionDescriptor.DisplayName ?? "Unknown action";

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(
                "Controller action {ActionName} returned HTTP {StatusCode} in {ElapsedMilliseconds} ms",
                actionName,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
        else if (statusCode >= StatusCodes.Status400BadRequest)
        {
            logger.LogWarning(
                "Controller action {ActionName} returned HTTP {StatusCode} in {ElapsedMilliseconds} ms",
                actionName,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
        else
        {
            logger.LogInformation(
                "Controller action {ActionName} returned HTTP {StatusCode} in {ElapsedMilliseconds} ms",
                actionName,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
