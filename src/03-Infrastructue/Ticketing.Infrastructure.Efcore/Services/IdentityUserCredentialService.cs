using Microsoft.AspNetCore.Identity;
using Ticketing.Application.Contracts.Services;
using Ticketing.Domain.Entites;

namespace Ticketing.Infrastructure.Efcore.Services;

public sealed class IdentityUserCredentialService(UserManager<AppUser> userManager) : IUserCredentialService
{
    public async Task<AppUser?> ValidateCredentialsAsync(
        string emailOrUserName,
        string password,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(emailOrUserName)
            ?? await userManager.FindByNameAsync(emailOrUserName);
        if (user is null)
            return null;

        return await userManager.CheckPasswordAsync(user, password) ? user : null;
    }
}
