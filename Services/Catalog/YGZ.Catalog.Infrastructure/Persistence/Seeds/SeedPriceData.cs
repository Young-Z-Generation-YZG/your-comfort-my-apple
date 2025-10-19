using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;


public static class SeedPriceData
{
    public static IEnumerable<IphoneSkuPrice> IphoneSkuPrices
    {
        get
        {
            ModelId modelId = ModelId.Of("664351e90087aa09993f5ae7");

            Model IPHONE_15 = Model.Create("IPHONE_15", 0);
            Model IPHONE_15_PLUS = Model.Create("IPHONE_15_PLUS", 1);

            Color BLUE = Color.Create("BLUE", "#D5DDDF", "iphone-15-finish-select-202309-6-1inch-blue_zgxzmz", 0);
            Color PINK = Color.Create("PINK", "#EBD3D4", "iphone-15-finish-select-202309-6-1inch-pink_j6v96t", 1);
            Color YELLOW = Color.Create("YELLOW", "#EDE6C8", "iphone-15-finish-select-202309-6-1inch-yellow_pwviwe", 2);
            Color GREEN = Color.Create("GREEN", "#D0D9CA", "iphone-15-finish-select-202309-6-1inch-green_yk0ln5", 3);
            Color BLACK = Color.Create("BLACK", "#4B4F50", "iphone-15-finish-select-202309-6-1inch-black_hhhvfs", 4);

            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            return new List<IphoneSkuPrice>
            {
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, BLUE, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(),  modelId, IPHONE_15, BLUE, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, BLUE, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, BLUE, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, PINK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, PINK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, PINK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, PINK, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, YELLOW, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, YELLOW, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, YELLOW, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, YELLOW, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, GREEN, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, GREEN, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, GREEN, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, GREEN, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, BLACK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, BLACK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, BLACK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15, BLACK, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLUE, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLUE, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLUE, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLUE, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, PINK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, PINK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, PINK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, PINK, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, YELLOW, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, YELLOW, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, YELLOW, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, YELLOW, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, GREEN, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, GREEN, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, GREEN, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, GREEN, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLACK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLACK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLACK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(SkuPriceId.Create(), modelId, IPHONE_15_PLUS, BLACK, STORAGE_1024, 1300),
            };
        }
    }

}

