using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public static class SeedIphoneModel
{
    public static IEnumerable<IphoneModel> IphoneModels
    {
        get
        {
            Category IPHONE_CATEGORY = Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null);

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

            List<Image> showcaseImages = new List<Image>
            {
                Image.Create("iphone-15-finish-select-202309-6-1inch-blue_zgxzmz", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp", "", "", 0, 0, 0, 0),
                Image.Create("iphone-15-finish-select-202309-6-1inch-pink_j6v96t", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp", "", "", 0, 0, 0, 1),
                Image.Create("iphone-15-finish-select-202309-6-1inch-yellow_pwviwe", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp", "", "", 0, 0, 0, 2),
                Image.Create("iphone-15-finish-select-202309-6-1inch-green_yk0ln5", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp", "", "", 0, 0, 0, 3),
                Image.Create("iphone-15-finish-select-202309-6-1inch-black_hhhvfs", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp", "", "", 0, 0, 0, 4),
            };

            AverageRating initAverageRating = AverageRating.Create(0, 0);
            List<RatingStar> initRatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };

            List<IphoneSkuPriceList> prices = new List<IphoneSkuPriceList>();
            List<Model> modelsInPrices = new()
            {
                Model.Create("IPHONE_15", 0),
                Model.Create("IPHONE_15_PLUS", 0)
            };

            List<Color> colorsInPrices = new()
            {
                Color.Create("BLUE", "", "", 0),
                Color.Create("PINK", "", "", 0),
                Color.Create("YELLOW", "", "", 0),
                Color.Create("GREEN", "", "", 0),
                Color.Create("BLACK", "", "", 0),
            };

            List<Storage> storagesInPrices = new()
            {
                Storage.Create("128GB", 128, 0),
                Storage.Create("256GB", 256, 0),
                Storage.Create("512GB", 512, 0),
                Storage.Create("1TB", 1024, 0),
            };
            // init
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            var IPHONE_15_15_PLUS_MODEL = IphoneModel.Create(iPhoneModelId: ModelId.Of("664351e90087aa09993f5ae7"),
                                              category: IPHONE_CATEGORY,
                                              name: "iPhone 15",
                                              models: new List<Model>
                                              {
                                                    IPHONE_15,
                                                    IPHONE_15_PLUS
                                              },
                                              colors: new List<Color>
                                              {
                                                    BLUE,
                                                    PINK,
                                                    YELLOW,
                                                    GREEN,
                                                    BLACK
                                              },
                                              storages: new List<Storage>
                                              {
                                                    STORAGE_128,
                                                    STORAGE_256,
                                                    STORAGE_512,
                                                    STORAGE_1024
                                              },
                                              prices: prices,
                                              showcaseImages: showcaseImages,
                                              description: "iPhone 15 model description.",
                                              averageRating: initAverageRating,
                                              ratingStars: initRatingStars);

            return new List<IphoneModel>
            {
                IPHONE_15_15_PLUS_MODEL
            };
        }
    }
}
