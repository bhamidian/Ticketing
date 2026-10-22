using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.Contracts.Services;
using Ticketing.Domain.Entites;
using Ticketing.Framework;
using Ticketing.Infrastructure.Efcore.Persistence;
using Ticketing.Infrastructure.Efcore.Repositories;
using Ticketing.Infrastructure.Efcore.Services;

namespace Ticketing.Infrastructure.Efcore;

public static class DependencyInjection
{
    public static async Task SeedDevelopmentDataAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>().SeedAsync();
    }

    public static IServiceCollection AddEfCorePersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddErrorDescriber<PersianIdentityErrorDescriber>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddEfCoreRepositories();
        return services;
    }

    public static IServiceCollection AddEfCoreRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketMessageRepository, TicketMessageRepository>();
        services.AddScoped<INormalUserRepository, NormalUserRepository>();
        services.AddScoped<IAgentRepository, AgentRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserCredentialService, IdentityUserCredentialService>();
        services.AddScoped<IUserRegistrationService, IdentityUserRegistrationService>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}
