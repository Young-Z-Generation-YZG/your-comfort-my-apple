
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Persistence.Configurations;
using YGZ.Catalog.Persistence.Services;

namespace YGZ.Catalog.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CatalogDbSetting>(configuration.GetSection(nameof(CatalogDbSetting)));

        services.AddMongoDbConfiguration();

        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
