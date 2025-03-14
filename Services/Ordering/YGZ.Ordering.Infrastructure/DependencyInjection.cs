using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Ordering.Infrastructure.Persistence;
using YGZ.Ordering.Infrastructure.Settings;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.BuildingBlocks.Messaging.Extensions;
using YGZ.Ordering.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Infrastructure.Persistence.Repositories;

namespace YGZ.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddOpenTelemetryExtensions();

        services.AddPostgresDatabase(configuration);

        services.AddQueuesFromApplicationLayer(configuration);

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IOrderRepository, OrderRepository>();

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

    public static IServiceCollection AddQueuesFromApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var queuesFromAssembly = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == "YGZ.Ordering.Application");

        services.AddMessageBrokerExtensions(configuration, queuesFromAssembly);

        return services;
    }
}
