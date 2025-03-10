

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Infrastructure.Persistence.Extensions;

public static class SeedDataExtensions
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var productItemRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<ProductItem>>();

        await SeedProductItemsAsync(productItemRepository);
    }

    private static async Task SeedProductItemsAsync(IMongoRepository<ProductItem> productItemRepository)
    {
        var existingItems = await productItemRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedData.ProductItems)
            {
                await productItemRepository.InsertOneAsync(item);
            }
        }
    }
}
