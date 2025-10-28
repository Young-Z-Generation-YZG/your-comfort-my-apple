using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Basket.Infrastructure.Persistence.Extensions;

public static class SeedDataExtension
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();


        await SeedAsync(scope);
    }

    public static async Task SeedAsync(IServiceScope scope)
    {
        // seed cached SKU prices
        //await SeedCachedSKUPrice(scope);

        // seed cached color images
        //await SeedCachedColorImages(scope);

        // seed cached model slugs
        //await SeedCachedModelSlugs(scope);
    }

    //public static async Task SeedCachedSKUPrice(IServiceScope scope)
    //{
    //    var skuDistributedCache = scope.ServiceProvider.GetRequiredService<ISKUPriceCache>();

    //    var skuPrices = SeedData.SKUPrices;

    //    await skuDistributedCache.SetPricesBatchAsync(skuPrices);
    //}

    //public static async Task SeedCachedColorImages(IServiceScope scope)
    //{
    //    var colorImageCache = scope.ServiceProvider.GetRequiredService<IColorImageCache>();

    //    var colorImages = SeedData.ColorImagesCache;

    //    await colorImageCache.SetImageUrlsBatchAsync(colorImages);
    //}

    //public static async Task SeedCachedModelSlugs(IServiceScope scope)
    //{
    //    var modelSlugCache = scope.ServiceProvider.GetRequiredService<IModelSlugCache>();

    //    var modelSlugs = SeedData.ModelSlugsCache;

    //    await modelSlugCache.SetSlugsBatchAsync(modelSlugs);
    //}
}
