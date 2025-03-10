using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Infrastructure.Persistence;
using YGZ.Basket.Infrastructure.Settings;
using YGZ.BuildingBlocks.Shared.Extensions;

namespace YGZ.Basket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddOpenTelemetryExtensions();

        services.AddPostgresDatabase(configuration);

        services.AddScoped<IBasketRepository, BasketRepository>();

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
}
