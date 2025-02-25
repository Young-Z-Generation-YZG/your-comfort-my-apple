using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Keycloak.Infrastructure.Extensions;
using YGZ.Keycloak.Infrastructure.Persistence;
using YGZ.Keycloak.Infrastructure.Settings;

namespace YGZ.Keycloak.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtensions(configuration);

        services.AddPostgresDatabase(configuration);

        services.AddIdentityExtension();

        services.AddOpenTelemetryExtensions();

        return services;
    }

    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.IdentityDb);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
