using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Services;

public interface IUserCredentialService
{
    Task<AppUser?> ValidateCredentialsAsync(string emailOrUserName, string password, CancellationToken cancellationToken = default);
}
