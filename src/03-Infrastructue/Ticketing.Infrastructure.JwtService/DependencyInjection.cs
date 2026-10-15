using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Infrastructure.JwtService.Contracts;
using Ticketing.Infrastructure.JwtService.Options;
using Ticketing.Infrastructure.JwtService.Services;

namespace Ticketing.Infrastructure.JwtService;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Issuer),
                "Jwt:Issuer is required."
            )
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Audience),
                "Jwt:Audience is required."
            )
            .Validate(
                options => Encoding.UTF8.GetByteCount(options.SigningKey) >= 32,
                "Jwt:SigningKey must contain at least 32 UTF-8 bytes."
            )
            .ValidateOnStart();
        services.AddScoped<global::Ticketing.Infrastructure.JwtService.Services.JwtService>();
        services.AddScoped<Ticketing.Application.Contracts.Services.IJwtService>(provider =>
            provider.GetRequiredService<global::Ticketing.Infrastructure.JwtService.Services.JwtService>()
        );
        services.AddScoped<Ticketing.Infrastructure.JwtService.Contracts.IJwtService>(provider =>
            provider.GetRequiredService<global::Ticketing.Infrastructure.JwtService.Services.JwtService>()
        );
        return services;
    }
}
