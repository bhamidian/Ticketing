using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Results;

namespace Ticketing.Endpoint.Webapi.Mapping;

public static class ApiResultMapper
{
    public static IActionResult ToProblem(ApplicationError error)
    {
        var statusCode = error.Type switch
        {
            ApplicationErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ApplicationErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ApplicationErrorType.NotFound => StatusCodes.Status404NotFound,
            ApplicationErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Type switch
            {
                ApplicationErrorType.Unauthorized => "احراز هویت لازم است",
                ApplicationErrorType.Forbidden => "دسترسی غیرمجاز",
                ApplicationErrorType.NotFound => "مورد درخواستی پیدا نشد",
                ApplicationErrorType.Conflict => "امکان انجام عملیات وجود ندارد",
                _ => "خطای غیرمنتظره‌ای رخ داد"
            },
            Detail = error.Message
        };
        problem.Extensions["code"] = error.Code;
        return new ObjectResult(problem) { StatusCode = statusCode };
    }
}
