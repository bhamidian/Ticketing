using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.Contracts.Services;
using Ticketing.Infrastructure.Efcore.Persistence;
using Ticketing.Infrastructure.Efcore.Repositories;
using Ticketing.Infrastructure.Efcore.Services;

namespace Ticketing.Infrastructure.Efcore;

public static class DependencyInjection
{
    public static IServiceCollection AddEfCoreRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketMessageRepository, TicketMessageRepository>();
        services.AddScoped<INormalUserRepository, NormalUserRepository>();
        services.AddScoped<IAgentRepository, AgentRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserCredentialService, IdentityUserCredentialService>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}
