
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Infrastructure.Persistence;
using YGZ.Catalog.Infrastructure.Persistence.Configurations;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddOpenTelemetryExtensions();

        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SettingKey));

        services.AddMongoDbConfigurations();

        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        return services;
    }
}
