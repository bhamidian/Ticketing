namespace Ticketing.Application.Exceptions;

public sealed class ForbiddenAccessException(string message) : Exception(message);
