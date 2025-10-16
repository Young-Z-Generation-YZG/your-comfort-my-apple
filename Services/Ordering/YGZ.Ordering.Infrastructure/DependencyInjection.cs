using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Messaging.Extensions;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Momo;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Vnpay;
using YGZ.Ordering.Infrastructure.Payments.Momo;
using YGZ.Ordering.Infrastructure.Payments.Vnpay;
using YGZ.Ordering.Infrastructure.Persistence;
using YGZ.Ordering.Infrastructure.Persistence.Interceptors;
using YGZ.Ordering.Infrastructure.Persistence.Repositories;
using YGZ.Ordering.Infrastructure.Settings;

namespace YGZ.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddKeycloakOpenTelemetryExtension();

        services.AddPostgresDatabase(configuration);

        services.AddQueuesFromApplicationLayer(configuration);

        services.Configure<VnpaySettings>(configuration.GetSection(VnpaySettings.SettingKey));
        services.Configure<MomoSettings>(configuration.GetSection(MomoSettings.SettingKey));
        services.Configure<WebClientSettings>(configuration.GetSection(WebClientSettings.SettingKey));

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        //services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddSingleton<IVnpayProvider, VnpayProvider>();
        services.AddSingleton<IMomoProvider, MomoProvider>();

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
