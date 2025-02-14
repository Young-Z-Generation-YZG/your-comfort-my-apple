using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Keycloak.Infrastructure.Extensions;

namespace YGZ.Keycloak.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtensions(configuration);

        services.AddOpenTelemetryExtensions();

        return services;
    }
}
