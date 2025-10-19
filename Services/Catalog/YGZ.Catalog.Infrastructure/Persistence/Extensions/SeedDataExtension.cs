using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;
using YGZ.Catalog.Infrastructure.Persistence.Seeds;

namespace YGZ.Catalog.Infrastructure.Persistence.Extensions;

public static class SeedDataExtensions
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var categoryRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<Category, CategoryId>>();
        var iphoneSkuPriceRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<IphoneSkuPrice, SkuPriceId>>();

        await SeedCategoriesAsync(categoryRepository);
        await SeedIphoneSkuPricesAsync(iphoneSkuPriceRepository);
    }

    private static async Task SeedCategoriesAsync(IMongoRepository<Category, CategoryId> categoryRepository)
    {
        var existingItems = await categoryRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedCategoryData.Categories)
            {
                await categoryRepository.InsertOneAsync(item);
            }
        }
    }

    private static async Task SeedIphoneSkuPricesAsync(IMongoRepository<IphoneSkuPrice, SkuPriceId> iphoneSkuPriceRepository)
    {
        var existingItems = await iphoneSkuPriceRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedPriceData.IphoneSkuPrices)
            {
                await iphoneSkuPriceRepository.InsertOneAsync(item);
            }
        }
    }
}
