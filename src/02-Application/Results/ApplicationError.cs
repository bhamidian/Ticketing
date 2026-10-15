namespace Ticketing.Application.Results;

public sealed record ApplicationError(string Code, string Message, ApplicationErrorType Type);
