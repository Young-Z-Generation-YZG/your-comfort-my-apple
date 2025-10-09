using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.Abstractions.Providers.Momo;
using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Infrastructure.Payments.Momo;
using YGZ.Basket.Infrastructure.Payments.Vnpay;
using YGZ.Basket.Infrastructure.Persistence.Repositories;
using YGZ.Basket.Infrastructure.Settings;
using YGZ.BuildingBlocks.Messaging.Extensions;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Basket.Infrastructure;

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

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();
        services.AddScoped<ISKUPriceCache, SKUPriceCacheRepository>();
        services.AddScoped<IColorImageCache, ColorImageCacheRepository>();
        services.AddScoped<IModelSlugCache, ModelSlugCacheRepository>();
        services.AddSingleton<IVnpayProvider, VnpayProvider>();
        services.AddSingleton<IMomoProvider, MomoProvider>();

        var redisConnectionString = configuration.GetConnectionString(ConnectionStrings.RedisDb)!;

        services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
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
