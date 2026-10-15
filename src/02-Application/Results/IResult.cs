namespace Ticketing.Application.Results;

public interface IResult
{
    bool IsSuccess { get; }
    ApplicationError? Error { get; }
}
