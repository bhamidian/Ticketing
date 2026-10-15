namespace Ticketing.Application.Results;

public sealed record Result<T>(bool IsSuccess, T? Value, ApplicationError? Error) : IResult
{
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(ApplicationError error) => new(false, default, error);
}
