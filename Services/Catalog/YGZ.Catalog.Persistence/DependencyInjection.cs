
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Persistence.Configurations;
using YGZ.Catalog.Persistence.Data;
using YGZ.Catalog.Persistence.Services;

namespace YGZ.Catalog.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CatalogDbSetting>(configuration.GetSection(nameof(CatalogDbSetting)));

        services.AddMongoDbConfiguration();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductItemService, ProductItemService>();
        services.AddScoped<IMongoContext, MongoContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();



        return services;
    }
}
