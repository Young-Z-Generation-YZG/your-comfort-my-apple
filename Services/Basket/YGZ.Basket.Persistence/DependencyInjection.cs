using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Persistence.Data;
using YGZ.Basket.Persistence.Helpers;
using YGZ.Basket.Persistence.Infrastructure;

namespace YGZ.Basket.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionString.BasketDb)!;

        services.AddSingleton(new ConnectionString(connectionString));

        services.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            options.Schema.For<ShoppingCart>().Identity(x => x.UserIdValue);
        }).UseLightweightSessions();

        services.JsonConverterHelper();

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString(ConnectionString.Redis)!;
        });

        return services;
    }
}
