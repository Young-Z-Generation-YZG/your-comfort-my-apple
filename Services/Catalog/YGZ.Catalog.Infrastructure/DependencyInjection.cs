
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Application.Core.Abstractions.Uploading;
using YGZ.Catalog.Domain.Core.Abstractions.Common;
using YGZ.Catalog.Infrastructure.Common;
using YGZ.Catalog.Infrastructure.Uploading;

namespace YGZ.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SettingKey));

        services.AddScoped<IUploadService, UploadService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
