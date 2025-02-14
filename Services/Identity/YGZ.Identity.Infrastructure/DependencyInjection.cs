using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Application.Abstractions;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Infrastructure.Extensions;
using YGZ.Identity.Infrastructure.Persistence;
using YGZ.Identity.Infrastructure.Persistence.Services;
using YGZ.Identity.Infrastructure.Services;
using YGZ.Identity.Infrastructure.Settings;

namespace YGZ.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStrings = configuration.GetConnectionString(ConnectionStrings.IdentityDb);

        // Add extensions
        services.AddIdentityExtension(configuration)
                .AddKeycloakIdentityServerExtension(configuration);

        // Add Database provider
        services.AddDatabaseProvider(connectionStrings!);

        // Add OpenTelemetry
        services.AddOpenTelemetryExtension();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingKey));

        // Register services
        services.AddScoped<ITokenService, TokenService>();
        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }

    public static IServiceCollection AddDatabaseProvider(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        return services;
    }
}
