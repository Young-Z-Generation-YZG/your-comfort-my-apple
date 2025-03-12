using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Ordering.Infrastructure.Persistence;
using YGZ.Ordering.Infrastructure.Settings;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        services.AddDbContext<OrderDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
