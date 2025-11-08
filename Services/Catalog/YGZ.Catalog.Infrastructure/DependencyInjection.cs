using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Messaging.Extensions;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Application.Abstractions.Data.Context;
using YGZ.Catalog.Application.Abstractions.Uploading;
using YGZ.Catalog.Infrastructure.Persistence.Configurations;
using YGZ.Catalog.Infrastructure.Persistence.Context;
using YGZ.Catalog.Infrastructure.Persistence.Interceptors;
using YGZ.Catalog.Infrastructure.Persistence.Repositories;
using YGZ.Catalog.Infrastructure.Services.ImageUploader;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakIdentityServerExtension(configuration);

        services.AddKeycloakOpenTelemetryExtension();

        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SettingKey));
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SettingKey));

        services.AddMongoDbConfigurations();

        services.AddScoped<ITransactionContext, TransactionContext>();
        services.AddScoped(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUploadImageService, UploadImageService>();

        services.AddScoped<IDispatchDomainEventInterceptor, DispatchDomainEventInterceptor>();

        services.AddQueuesFromApplicationLayer(configuration);

        var redisConnectionString = configuration.GetConnectionString(ConnectionStrings.RedisDb)!;

        services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        return services;
    }

    public static IServiceCollection AddQueuesFromApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var queuesFromAssembly = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(asm => asm.GetName().Name == "YGZ.Catalog.Application");

        services.AddMessageBrokerExtensions(configuration, queuesFromAssembly);

        return services;
    }
}
