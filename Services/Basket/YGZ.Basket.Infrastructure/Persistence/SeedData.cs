using YGZ.Basket.Domain.Cache.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Infrastructure.Persistence;

public static class SeedData
{
    public static IEnumerable<PriceCache> SKUPrices
    {
        get
        {
            List<Model> models = new()
            {
                Model.Create("IPHONE_15"),
                Model.Create("IPHONE_15_PLUS")
            };

            List<Color> colors = new()
            {
                Color.Create("BLUE"),
                Color.Create("PINK"),
                Color.Create("YELLOW"),
                Color.Create("GREEN"),
                Color.Create("BLACK"),
            };

            List<Storage> storages = new()
            {
                Storage.Create("128GB"),
                Storage.Create("256GB"),
                Storage.Create("512GB"),
                Storage.Create("1TB"),
            };

            List<PriceCache> skuPrices = new();

            skuPrices.Add(PriceCache.Create(models[0], colors[0], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[0], colors[0], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[0], colors[0], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[0], colors[0], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[0], colors[1], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[0], colors[1], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[0], colors[1], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[0], colors[1], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[0], colors[2], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[0], colors[2], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[0], colors[2], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[0], colors[2], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[0], colors[3], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[0], colors[3], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[0], colors[3], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[0], colors[3], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[0], colors[4], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[0], colors[4], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[0], colors[4], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[0], colors[4], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[1], colors[0], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[1], colors[0], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[1], colors[0], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[1], colors[0], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[1], colors[1], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[1], colors[1], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[1], colors[1], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[1], colors[1], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[1], colors[2], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[1], colors[2], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[1], colors[2], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[1], colors[2], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[1], colors[3], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[1], colors[3], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[1], colors[3], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[1], colors[3], storages[3], 1300));

            skuPrices.Add(PriceCache.Create(models[1], colors[4], storages[0], 1000));
            skuPrices.Add(PriceCache.Create(models[1], colors[4], storages[1], 1100));
            skuPrices.Add(PriceCache.Create(models[1], colors[4], storages[2], 1200));
            skuPrices.Add(PriceCache.Create(models[1], colors[4], storages[3], 1300));

            return skuPrices;
        }
    }

    public static IEnumerable<ColorImageCache> ColorImagesCache
    {
        get
        {
            List<Color> colors = new()
            {
                Color.Create("BLUE"),
                Color.Create("PINK"),
                Color.Create("YELLOW"),
                Color.Create("GREEN"),
                Color.Create("BLACK"),
            };

            List<ColorImageCache> colorImageCaches = new();

            colorImageCaches.Add(ColorImageCache.Create("68e403d5617b27ad030bf28f", colors[0], "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp"));
            colorImageCaches.Add(ColorImageCache.Create("68e403d5617b27ad030bf28f", colors[1], "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp"));
            colorImageCaches.Add(ColorImageCache.Create("68e403d5617b27ad030bf28f", colors[2], "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp"));
            colorImageCaches.Add(ColorImageCache.Create("68e403d5617b27ad030bf28f", colors[3], "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp"));
            colorImageCaches.Add(ColorImageCache.Create("68e403d5617b27ad030bf28f", colors[4], "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp"));

            return colorImageCaches;
        }
    }

    public static IEnumerable<ModelSlugCache> ModelSlugsCache
    {
        get
        {
            List<ModelSlugCache> modelSlugsCache = new();

            modelSlugsCache.Add(ModelSlugCache.Create("68e403d5617b27ad030bf28f", "iphone-15"));

            return modelSlugsCache;
        }
    }
}
