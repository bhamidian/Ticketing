using MediatR;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Results;
using FluentValidation;

namespace Ticketing.Application.Behaviors;

public sealed class ApplicationLoggingBehavior<TRequest, TResponse>(
    ILogger<ApplicationLoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();
            if (response is IResult { IsSuccess: false, Error: not null } result)
            {
                logger.LogWarning(
                    "Application request {RequestName} returned business error {ErrorCode} ({ErrorType})",
                    typeof(TRequest).Name,
                    result.Error.Code,
                    result.Error.Type);
            }

            return response;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (IsExpectedFailure(exception))
        {
            logger.LogWarning(
                "Application request {RequestName} was rejected with {FailureType}",
                typeof(TRequest).Name,
                exception.GetType().Name);
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Application request {RequestName} failed unexpectedly",
                typeof(TRequest).Name);
            throw;
        }
    }

    private static bool IsExpectedFailure(Exception exception) => exception is FluentValidation.ValidationException;
}
