using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Contracts.Services;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Services;

public sealed class IdentityUserRegistrationService(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext) : IUserRegistrationService
{
    public async Task<UserRegistrationResult> RegisterNormalUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var user = new AppUser { Id = Guid.NewGuid(), UserName = email, Email = email };
        var created = await userManager.CreateAsync(user, password);
        if (!created.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new UserRegistrationResult(null, created.Errors.Select(error => error.Code).ToArray());
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new InvalidOperationException("The default User role could not be assigned during registration.");
        }

        var now = DateTime.UtcNow;
        var departmentId = await dbContext.Departments
            .Where(department => department.Name == "پشتیبانی")
            .Select(department => (Guid?)department.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (departmentId is null)
            throw new InvalidOperationException("Default support department is missing. Apply database migrations before registering users.");

        dbContext.NormalUsers.Add(new NormalUser
        {
            Id = Guid.NewGuid(),
            AppUserId = user.Id,
            DepartmentId = departmentId.Value,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedById = user.Id,
            UpdatedById = user.Id
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return new UserRegistrationResult(user, []);
    }
}
