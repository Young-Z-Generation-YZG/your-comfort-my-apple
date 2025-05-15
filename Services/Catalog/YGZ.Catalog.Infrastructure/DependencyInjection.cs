
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Application.Abstractions.Uploading;
using YGZ.Catalog.Infrastructure.Persistence.Configurations;
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

        services.AddKeycloakOpenTelemetryExtensions();

        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SettingKey));
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SettingKey));

        services.AddMongoDbConfigurations();

        services.AddScoped(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));
        services.AddScoped<IIPhone16ModelRepository, IPhone16ModelRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUploadImageService, UploadImageService>();

        services.AddScoped<IDispatchDomainEventInterceptor, DispatchDomainEventInterceptor>();

        return services;
    }
}
