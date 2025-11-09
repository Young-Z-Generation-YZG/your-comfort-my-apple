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

            Color BLUE = Color.Create("Blue", "#D5DDDF", "iphone-15-finish-select-202309-6-1inch-blue_zgxzmz", 0);
            Color PINK = Color.Create("Pink", "#EBD3D4", "iphone-15-finish-select-202309-6-1inch-pink_j6v96t", 1);
            Color YELLOW = Color.Create("Yellow", "#EDE6C8", "iphone-15-finish-select-202309-6-1inch-yellow_pwviwe", 2);
            Color GREEN = Color.Create("Green", "#D0D9CA", "iphone-15-finish-select-202309-6-1inch-green_yk0ln5", 3);
            Color BLACK = Color.Create("Black", "#4B4F50", "iphone-15-finish-select-202309-6-1inch-black_hhhvfs", 4);

            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            List<SkuPriceId> skus = new List<SkuPriceId>();

            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f5f"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f60"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f61"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f62"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f63"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f64"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f65"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f66"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f67"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f68"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f69"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f6a"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f6b"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f6c"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f6d"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f6e"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f6f"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f70"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f71"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f72"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f73"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f74"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f75"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f76"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f77"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f78"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f79"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f7a"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f7b"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f7c"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f7d"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f7e"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f7f"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f80"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f81"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f82"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f83"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f84"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f85"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f86"));

            return new List<IphoneSkuPrice>
            {
                IphoneSkuPrice.Create(skus[0], modelId, IPHONE_15, BLUE, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[1],  modelId, IPHONE_15, BLUE, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[2], modelId, IPHONE_15, BLUE, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[3], modelId, IPHONE_15, BLUE, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[4], modelId, IPHONE_15, PINK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[5], modelId, IPHONE_15, PINK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[6], modelId, IPHONE_15, PINK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[7], modelId, IPHONE_15, PINK, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[8], modelId, IPHONE_15, YELLOW, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[9], modelId, IPHONE_15, YELLOW, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[10], modelId, IPHONE_15, YELLOW, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[11], modelId, IPHONE_15, YELLOW, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[12], modelId, IPHONE_15, GREEN, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[13], modelId, IPHONE_15, GREEN, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[14], modelId, IPHONE_15, GREEN, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[15], modelId, IPHONE_15, GREEN, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[16], modelId, IPHONE_15, BLACK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[17], modelId, IPHONE_15, BLACK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[18], modelId, IPHONE_15, BLACK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[19], modelId, IPHONE_15, BLACK, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[20], modelId, IPHONE_15_PLUS, BLUE, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[21], modelId, IPHONE_15_PLUS, BLUE, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[22], modelId, IPHONE_15_PLUS, BLUE, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[23], modelId, IPHONE_15_PLUS, BLUE, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[24], modelId, IPHONE_15_PLUS, PINK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[25], modelId, IPHONE_15_PLUS, PINK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[26], modelId, IPHONE_15_PLUS, PINK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[27], modelId, IPHONE_15_PLUS, PINK, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[28], modelId, IPHONE_15_PLUS, YELLOW, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[29], modelId, IPHONE_15_PLUS, YELLOW, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[30], modelId, IPHONE_15_PLUS, YELLOW, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[31], modelId, IPHONE_15_PLUS, YELLOW, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[32], modelId, IPHONE_15_PLUS, GREEN, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[33], modelId, IPHONE_15_PLUS, GREEN, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[34], modelId, IPHONE_15_PLUS, GREEN, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[35], modelId, IPHONE_15_PLUS, GREEN, STORAGE_1024, 1300),
                IphoneSkuPrice.Create(skus[36], modelId, IPHONE_15_PLUS, BLACK, STORAGE_128, 1000),
                IphoneSkuPrice.Create(skus[37], modelId, IPHONE_15_PLUS, BLACK, STORAGE_256, 1100),
                IphoneSkuPrice.Create(skus[38], modelId, IPHONE_15_PLUS, BLACK, STORAGE_512, 1200),
                IphoneSkuPrice.Create(skus[39], modelId, IPHONE_15_PLUS, BLACK, STORAGE_1024, 1300),
            };
        }
    }

}

