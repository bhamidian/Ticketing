using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Services;

public sealed record UserRegistrationResult(AppUser? User, IReadOnlyList<string> ErrorCodes)
{
    public bool Succeeded => User is not null;
}

public interface IUserRegistrationService
{
    Task<UserRegistrationResult> RegisterNormalUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
