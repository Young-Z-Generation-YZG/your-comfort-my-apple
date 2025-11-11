using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Catalog.Domain.Tenants;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;
using YGZ.Catalog.Infrastructure.Persistence.Seeds;

namespace YGZ.Catalog.Infrastructure.Persistence.Extensions;

public static class SeedDataExtension
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var categoryRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<Category, CategoryId>>();
        var iphoneModelRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<IphoneModel, ModelId>>();
        var iphoneSkuPriceRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<IphoneSkuPrice, SkuPriceId>>();
        var tenantRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<Tenant, TenantId>>();
        var branchRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<Branch, BranchId>>();
        var skuRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<SKU, SkuId>>();
        var distributedCache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();

        await SeedCategoriesAsync(categoryRepository);

        await SeedIphoneModelsAsync(iphoneModelRepository);
        // await SeedIphoneSkuPricesAsync(iphoneSkuPriceRepository);


        // cache data
        //await SeedCacheIphoneSkuPrices(iphoneSkuPriceRepository, distributedCache);
        //await SeedCacheColorImages(iphoneModelRepository, distributedCache);



        //await SeedTenantsAsync(tenantRepository);

        //await SeedSkusAsync(skuRepository);
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

    private static async Task SeedTenantsAsync(IMongoRepository<Tenant, TenantId> tenantRepository)
    {
        var existingItems = await tenantRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedTenantData.Tenants)
            {
                await tenantRepository.InsertOneAsync(item);
            }
        }
    }

    private static async Task SeedBranchesAsync(IMongoRepository<Branch, BranchId> branchRepository)
    {
        var existingItems = await branchRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedTenantData.Branches)
            {
                await branchRepository.InsertOneAsync(item);
            }
        }
    }

    private static async Task SeedIphoneModelsAsync(IMongoRepository<IphoneModel, ModelId> iphoneModelRepository)
    {
        var existingItems = await iphoneModelRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedIphoneModel.Iphone15Models)
            {
                await iphoneModelRepository.InsertOneAsync(item);
            }

            foreach (var item in SeedIphoneModel.Iphone16Models)
            {
                await iphoneModelRepository.InsertOneAsync(item);
            }

            foreach (var item in SeedIphoneModel.Iphone16EModels)
            {
                await iphoneModelRepository.InsertOneAsync(item);
            }

            foreach (var item in SeedIphoneModel.Iphone17Models)
            {
                await iphoneModelRepository.InsertOneAsync(item);
            }

            foreach (var item in SeedIphoneModel.Iphone17ProModels)
            {
                await iphoneModelRepository.InsertOneAsync(item);
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

    // seed cache data
    private static async Task SeedCacheIphoneSkuPrices(IMongoRepository<IphoneSkuPrice, SkuPriceId> iphoneSkuPriceRepository, IDistributedCache distributedCache)
    {
        var iphoneSkuPrices = await iphoneSkuPriceRepository.GetAllAsync();

        foreach (var iphoneSkuPrice in iphoneSkuPrices)
        {
            await distributedCache.SetStringAsync(iphoneSkuPrice.CachedKey, iphoneSkuPrice.UnitPrice.ToString());
        }
    }

    private static async Task SeedCacheColorImages(IMongoRepository<IphoneModel, ModelId> iphoneModelRepository, IDistributedCache distributedCache)
    {
        var iphoneModels = await iphoneModelRepository.GetAllAsync();

        foreach (var iphoneModel in iphoneModels)
        {
            var colors = iphoneModel.Colors;
            var displayImages = iphoneModel.ShowcaseImages;

            foreach (var color in colors)
            {
                var matchedDisplayImageUrl = displayImages.FirstOrDefault(di => di.ImageId == color.ShowcaseImageId);

                EColor.TryFromName(color.NormalizedName, out var colorEnum);

                await distributedCache.SetStringAsync(CacheKeyPrefixConstants.CatalogService.GetDisplayImageUrlKey(iphoneModel.Id.Value!, colorEnum), matchedDisplayImageUrl!.ImageUrl);
            }

        }
    }
}
