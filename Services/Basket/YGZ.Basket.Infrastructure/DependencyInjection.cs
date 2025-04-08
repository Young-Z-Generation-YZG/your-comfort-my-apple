using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Infrastructure.Settings;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.BuildingBlocks.Messaging.Extensions;
using YGZ.Basket.Infrastructure.Persistence.Repositories;
using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.Basket.Infrastructure.Payments.Vnpay;
using YGZ.Basket.Infrastructure.Payments.Momo;
using YGZ.Basket.Application.Abstractions.Providers.Momo;

namespace YGZ.Basket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddKeycloakOpenTelemetryExtensions();

        services.AddPostgresDatabase(configuration);

        services.AddQueuesFromApplicationLayer(configuration);

        services.Configure<VnpaySettings>(configuration.GetSection(VnpaySettings.SettingKey));
        services.Configure<MomoSettings>(configuration.GetSection(MomoSettings.SettingKey));
        services.Configure<WebClientSettings>(configuration.GetSection(WebClientSettings.SettingKey));

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();
        services.AddSingleton<IVnpayProvider, VnpayProvider>();
        services.AddSingleton<IMomoProvider, MomoProvider>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString(ConnectionStrings.RedisDb);
        });

        return services;
    }

    public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.BasketDb)!;

        services.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            options.Schema.For<ShoppingCart>().Identity(x => x.UserEmail);

        }).UseLightweightSessions();

        return services;
    }

    public static IServiceCollection AddQueuesFromApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var queuesFromAssembly = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == "YGZ.Basket.Application");

        services.AddMessageBrokerExtensions(configuration, queuesFromAssembly);

        return services;
    }
}
