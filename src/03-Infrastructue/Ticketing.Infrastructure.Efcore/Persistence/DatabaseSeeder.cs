using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ticketing.Domain.Entites;
using Ticketing.Domain.Enums;

namespace Ticketing.Infrastructure.Efcore.Persistence;

public sealed class DatabaseSeeder(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IConfiguration configuration,
    ILogger<DatabaseSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var seedUsers = configuration.GetSection("SeedUsers:Accounts").GetChildren().ToArray();
        var userCount = seedUsers.Count(section => string.Equals(section["Role"], "User", StringComparison.OrdinalIgnoreCase));
        var agentCount = seedUsers.Count(section => string.Equals(section["Role"], "Agent", StringComparison.OrdinalIgnoreCase));

        if (userCount != 3 || agentCount != 3)
            throw new InvalidOperationException("Runtime seeding requires exactly three User accounts and three Agent accounts.");

        if (!await dbContext.Database.CanConnectAsync(cancellationToken))
            throw new InvalidOperationException("Cannot connect to the configured database. Apply the database migrations before runtime seeding.");

        foreach (var roleName in new[] { "User", "Agent" })
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            EnsureSucceeded(await roleManager.CreateAsync(new IdentityRole<Guid>(roleName)
            {
                Id = Guid.NewGuid()
            }), $"creating role '{roleName}'");
        }

        foreach (var section in seedUsers)
        {
            var email = section["Email"];
            var password = section["Password"];
            var roleName = section["Role"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(roleName))
                throw new InvalidOperationException($"Seed account '{section.Key}' must define Email, Password, and Role.");

            var account = await userManager.FindByEmailAsync(email);
            if (account is null)
            {
                account = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                EnsureSucceeded(await userManager.CreateAsync(account, password), $"creating seed account '{email}'");
                logger.LogInformation("Created runtime seed account {Email}", email);
            }

            if (!await userManager.IsInRoleAsync(account, roleName))
                EnsureSucceeded(await userManager.AddToRoleAsync(account, roleName), $"assigning role '{roleName}' to '{email}'");

            if (roleName.Equals("User", StringComparison.OrdinalIgnoreCase) &&
                !await dbContext.NormalUsers.AnyAsync(profile => profile.AppUserId == account.Id, cancellationToken))
            {
                var now = DateTime.UtcNow;
                dbContext.NormalUsers.Add(new NormalUser
                {
                    Id = Guid.NewGuid(),
                    AppUserId = account.Id,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedById = account.Id,
                    UpdatedById = account.Id
                });
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            if (roleName.Equals("Agent", StringComparison.OrdinalIgnoreCase) &&
                !await dbContext.Agents.AnyAsync(profile => profile.AppUserId == account.Id, cancellationToken))
                await EnsureAgentProfileAsync(account, cancellationToken);
        }

        await SeedPaginationTicketsAsync(seedUsers, cancellationToken);

        logger.LogInformation("Runtime identity seeding completed for six configured accounts");
    }

    private async Task SeedPaginationTicketsAsync(IConfigurationSection[] seedUsers, CancellationToken cancellationToken)
    {
        var subjects = new[]
        {
            "مشکل در ورود به حساب کاربری", "پیگیری وضعیت سفارش", "درخواست اصلاح اطلاعات حساب",
            "پرداخت انجام شده اما سفارش ثبت نشده", "بازیابی گذرواژه", "دریافت فاکتور خرید",
            "خطا هنگام ثبت سفارش", "به‌روزرسانی شماره تماس", "پیگیری بازگشت وجه",
            "مشکل در دریافت کد تأیید", "تغییر نشانی ارسال", "مغایرت مبلغ پرداختی",
            "درخواست راهنمایی دربارهٔ حساب", "سفارش در فهرست نمایش داده نمی‌شود", "مشکل در دانلود رسید"
        };
        var users = new List<(AppUser Account, NormalUser Profile)>();

        foreach (var section in seedUsers.Where(section => string.Equals(section["Role"], "User", StringComparison.OrdinalIgnoreCase)))
        {
            var email = section["Email"];
            if (string.IsNullOrWhiteSpace(email))
                continue;

            var account = await userManager.FindByEmailAsync(email);
            if (account is null)
                continue;

            var profile = await dbContext.NormalUsers.SingleOrDefaultAsync(item => item.AppUserId == account.Id, cancellationToken);
            if (profile is not null)
                users.Add((account, profile));
        }

        if (users.Count == 0)
            throw new InvalidOperationException("Pagination sample tickets require at least one seeded User account.");

        var now = DateTime.UtcNow;
        var addedCount = 0;
        for (var index = 0; index < subjects.Length; index++)
        {
            var ticketId = Guid.Parse($"80000000-0000-0000-0000-{index + 1:000000000000}");
            if (await dbContext.Tickets.AnyAsync(ticket => ticket.Id == ticketId, cancellationToken))
                continue;

            var (account, profile) = users[index % users.Count];
            var createdAt = now.AddMinutes(-index);
            var ticket = new Ticket
            {
                Id = ticketId,
                Subject = subjects[index],
                TicketLifeCycle = TicketLifeCycleEnum.OPEN,
                TicketPriority = (TicketPriorityEnum)(index % 3),
                NormalUserId = profile.Id,
                AssignedAgentId = null,
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                CreatedById = account.Id,
                UpdatedById = account.Id
            };
            ticket.TicketMessages.Add(new TicketMessage
            {
                Id = Guid.Parse($"90000000-0000-0000-0000-{index + 1:000000000000}"),
                TicketId = ticket.Id,
                Ticket = ticket,
                SenderId = account.Id,
                Message = $"سلام، لطفاً دربارهٔ «{ticket.Subject}» راهنمایی کنید.",
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                CreatedById = account.Id,
                UpdatedById = account.Id
            });

            dbContext.Tickets.Add(ticket);
            addedCount++;
        }

        if (addedCount > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Created {Count} unassigned new tickets for pagination preview", addedCount);
        }
    }

    private async Task EnsureAgentProfileAsync(AppUser account, CancellationToken cancellationToken)
    {
        if (await dbContext.Agents.AnyAsync(profile => profile.AppUserId == account.Id, cancellationToken))
            return;

        var now = DateTime.UtcNow;
        dbContext.Agents.Add(new Agent
        {
            Id = Guid.NewGuid(),
            AppUserId = account.Id,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedById = account.Id,
            UpdatedById = account.Id
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureSucceeded(IdentityResult result, string operation)
    {
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(error => $"{error.Code}: {error.Description}"));
            throw new InvalidOperationException($"Identity failed while {operation}: {errors}");
        }
    }
}
