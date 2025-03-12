using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Ordering.Infrastructure.Persistence;
using YGZ.Ordering.Infrastructure.Settings;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddOpenTelemetryExtensions();

        services.AddPostgresDatabase(configuration);

        return services;
    }

    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.OrderingDb);

        services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
