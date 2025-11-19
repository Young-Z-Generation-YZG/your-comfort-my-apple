using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public static class SeedIphoneModel
{
    public static IEnumerable<IphoneModel> Iphone15Models
    {
        get
        {
            Category IPHONE_CATEGORY = Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null, parentCategory: null);

            Model IPHONE_15 = Model.Create("iPhone 15", 0);
            Model IPHONE_15_PLUS = Model.Create("iPhone 15 Plus", 1);

            // Model IPHONE_16_PRO = Model.Create("IPHONE_16_PRO", 0);
            // Model IPHONE_16_PRO_MAX = Model.Create("IPHONE_16_PRO_MAX", 1);


            // Seed colors
            Color BLUE_15 = Color.Create("Blue", "#D5DDDF", "iphone-15-finish-select-202309-6-1inch-blue_zgxzmz", 0);
            Color PINK_15 = Color.Create("Pink", "#EBD3D4", "iphone-15-finish-select-202309-6-1inch-pink_j6v96t", 1);
            Color YELLOW_15 = Color.Create("Yellow", "#EDE6C8", "iphone-15-finish-select-202309-6-1inch-yellow_pwviwe", 2);
            Color GREEN_15 = Color.Create("Green", "#D0D9CA", "iphone-15-finish-select-202309-6-1inch-green_yk0ln5", 3);
            Color BLACK_15 = Color.Create("Black", "#4B4F50", "iphone-15-finish-select-202309-6-1inch-black_hhhvfs", 4);


            // Seed storages
            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);


            // Seed showcase images
            List<Image> showcaseImages = new List<Image>
            {
                Image.Create("iphone-15-finish-select-202309-6-1inch-blue_zgxzmz", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp", "", "", 0, 0, 0, 0),
                Image.Create("iphone-15-finish-select-202309-6-1inch-pink_j6v96t", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp", "", "", 0, 0, 0, 1),
                Image.Create("iphone-15-finish-select-202309-6-1inch-yellow_pwviwe", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp", "", "", 0, 0, 0, 2),
                Image.Create("iphone-15-finish-select-202309-6-1inch-green_yk0ln5", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp", "", "", 0, 0, 0, 3),
                Image.Create("iphone-15-finish-select-202309-6-1inch-black_hhhvfs", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp", "", "", 0, 0, 0, 4),
            };


            // Seed average rating and rating stars
            AverageRating initAverageRating = AverageRating.Create(0, 0);
            List<RatingStar> initRatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };

            List<Model> modelsInPrices = new()
            {
                IPHONE_15,
                IPHONE_15_PLUS
            };

            List<Color> colorsInPrices = new()
            {
                BLUE_15,
                PINK_15,
                YELLOW_15,
                GREEN_15,
                BLACK_15,
            };

            List<Storage> storagesInPrices = new()
            {
                STORAGE_128,
                STORAGE_256,
                STORAGE_512,
                STORAGE_1024,
            };


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

            // init
            List<SkuPriceList> prices = new List<SkuPriceList>();

            prices.Add(SkuPriceList.Create(skus[0].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[1].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[2].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[3].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[4].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[5].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[6].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[7].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[8].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[9].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[10].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[11].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[12].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[13].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[14].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[15].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[16].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[17].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[18].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[19].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[20].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[21].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[22].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[23].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[24].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[25].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[26].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[27].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[28].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[29].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[30].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[31].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[32].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[33].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[34].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[35].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

            prices.Add(SkuPriceList.Create(skus[36].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
            prices.Add(SkuPriceList.Create(skus[37].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
            prices.Add(SkuPriceList.Create(skus[38].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[39].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

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
                                                    BLUE_15,
                                                    PINK_15,
                                                    YELLOW_15,
                                                    GREEN_15,
                                                    BLACK_15
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
                                              ratingStars: initRatingStars,
                                              isNewest: false);

            return new List<IphoneModel>
            {
                IPHONE_15_15_PLUS_MODEL
            };
        }
    }

    public static IEnumerable<IphoneModel> Iphone16Models
    {
        get
        {
            Model IPHONE_16 = Model.Create("iPhone 16", 0);
            Model IPHONE_16_PLUS = Model.Create("iPhone 16 Plus", 1);

            Color ULTRAMARINE_16 = Color.Create("Ultramarine", "#A0B3F7", "iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08", 0);
            Color TEAL_16 = Color.Create("Teal", "#B4D6D5", "iphone-16-finish-select-202409-6-1inch-teal_gfumfa", 1);
            Color PINK_16 = Color.Create("Pink", "#F2B5DD", "iphone-16-finish-select-202409-6-1inch-pink_q2saue", 2);
            Color WHITE_16 = Color.Create("White", "#FAFAFA", "iphone-16-finish-select-202409-6-1inch-white_ghumel", 3);
            Color BLACK_16 = Color.Create("Black", "#484C4E", "iphone-16-finish-select-202409-6-1inch-black_wnfzl5", 4);

            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            List<Image> showcaseImages = new List<Image>
            {
                Image.Create("iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp", "", "", 0, 0, 0, 0),
                Image.Create("iphone-16-finish-select-202409-6-1inch-teal_gfumfa", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811404/iphone-16-finish-select-202409-6-1inch-teal_gfumfa.webp", "", "", 0, 0, 0, 1),
                Image.Create("iphone-16-finish-select-202409-6-1inch-pink_q2saue", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811098/iphone-16-finish-select-202409-6-1inch-pink_q2saue.webp", "", "", 0, 0, 0, 2),
                Image.Create("iphone-16-finish-select-202409-6-1inch-white_ghumel", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811448/iphone-16-finish-select-202409-6-1inch-white_ghumel.webp", "", "", 0, 0, 0, 3),
                Image.Create("iphone-16-finish-select-202409-6-1inch-black_wnfzl5", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811496/iphone-16-finish-select-202409-6-1inch-black_wnfzl5.webp", "", "", 0, 0, 0, 4),
            };


            // Seed average rating and rating stars
            AverageRating initAverageRating = AverageRating.Create(0, 0);
            List<RatingStar> initRatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };


            // Seed models in prices
            List<Model> modelsInPrices = new()
            {
                IPHONE_16,
                IPHONE_16_PLUS
            };


            // Seed colors in prices
            List<Color> colorsInPrices = new()
            {
                ULTRAMARINE_16,
                TEAL_16,
                PINK_16,
                WHITE_16,
                BLACK_16
            };


            // Seed storages in prices
            List<Storage> storagesInPrices = new()
            {
                STORAGE_128,
                STORAGE_256,
                STORAGE_512,
                STORAGE_1024
            };


            // Seed skus
            List<SkuPriceId> skus = new List<SkuPriceId>();
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f87"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f88"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f89"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f8a"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f8b"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f8c"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f8d"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f8e"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f8f"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f90"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f91"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f92"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f93"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f94"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f95"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f96"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f97"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f98"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f99"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f9a"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f9b"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f9c"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f9d"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f9e"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23f9f"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa0"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa1"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa2"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa3"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa4"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa5"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa6"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa7"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa8"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fa9"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23faa"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fab"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fac"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fad"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fae"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23faf"));



            // Seed sku prices
            List<SkuPriceList> prices = new List<SkuPriceList>();

            // Model 0 (IPHONE_16), Color 0 (ULTRAMARINE_16)
            prices.Add(SkuPriceList.Create(skus[0].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[1].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[2].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[3].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 0 (IPHONE_16), Color 1 (TEAL_16)
            prices.Add(SkuPriceList.Create(skus[4].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[5].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[6].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[7].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 0 (IPHONE_16), Color 2 (PINK_16)
            prices.Add(SkuPriceList.Create(skus[8].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[9].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[10].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[11].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 0 (IPHONE_16), Color 3 (WHITE_16)
            prices.Add(SkuPriceList.Create(skus[12].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[13].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[14].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[15].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 0 (IPHONE_16), Color 4 (BLACK_16)
            prices.Add(SkuPriceList.Create(skus[16].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[17].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[18].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[19].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 1 (IPHONE_16_PLUS), Color 0 (ULTRAMARINE_16)
            prices.Add(SkuPriceList.Create(skus[20].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[21].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[22].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[23].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 1 (IPHONE_16_PLUS), Color 1 (TEAL_16)
            prices.Add(SkuPriceList.Create(skus[24].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[25].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[26].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[27].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 1 (IPHONE_16_PLUS), Color 2 (PINK_16)
            prices.Add(SkuPriceList.Create(skus[28].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[29].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[30].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[31].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 1 (IPHONE_16_PLUS), Color 3 (WHITE_16)
            prices.Add(SkuPriceList.Create(skus[32].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[33].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[34].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[35].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 1 (IPHONE_16_PLUS), Color 4 (BLACK_16)
            prices.Add(SkuPriceList.Create(skus[36].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[37].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[38].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[39].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1500));


            // Seed iPhone category
            Category IPHONE_CATEGORY = Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null, parentCategory: null);

            // Seed Iphone 16 model
            var IPHONE_16_16_PLUS_MODEL = IphoneModel.Create(iPhoneModelId: ModelId.Of("6643543d0087aa09993f5b14"),
                                              category: IPHONE_CATEGORY,
                                              name: "iPhone 16",
                                              models: new List<Model>
                                              {
                                                    IPHONE_16,
                                                    IPHONE_16_PLUS
                                              },
                                              colors: new List<Color>
                                              {
                                                    ULTRAMARINE_16,
                                                    TEAL_16,
                                                    PINK_16,
                                                    WHITE_16,
                                                    BLACK_16
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
                                              description: "iPhone 16 model description.",
                                              averageRating: initAverageRating,
                                              ratingStars: initRatingStars,
                                              isNewest: false);

            return new List<IphoneModel>
            {
                IPHONE_16_16_PLUS_MODEL
            };
        }
    }

    public static IEnumerable<IphoneModel> Iphone16EModels
    {
        get
        {
            Model IPHONE_16_E = Model.Create("iPhone 16e", 0);



            Color WHITE_16_E = Color.Create("White", "#FAFAFA", "iphone-16e-finish-select-202502-white_g1coja", 0);
            Color BLACK_16_E = Color.Create("Black", "#4B4F50", "iphone-16e-finish-select-202502-black_yq48ki", 1);


            // Seed storages
            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            // Seed showcase images
            List<Image> showcaseImages = new List<Image>
            {
                Image.Create("iphone-16e-finish-select-202502-white_g1coja", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-16e-finish-select-202502-white_g1coja.webp", "", "", 0, 0, 0, 0),
                Image.Create("iphone-16e-finish-select-202502-black_yq48ki", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-16e-finish-select-202502-black_yq48ki.webp", "", "", 0, 0, 0, 1),
            };

            // Seed average rating and rating stars
            AverageRating initAverageRating = AverageRating.Create(0, 0);
            List<RatingStar> initRatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };

            // Seed models in prices
            List<Model> modelsInPrices = new()
            {
                IPHONE_16_E,
            };

            // Seed colors in prices
            List<Color> colorsInPrices = new()
            {
                WHITE_16_E,
                BLACK_16_E,
            };

            // Seed storages in prices
            List<Storage> storagesInPrices = new()
            {
                STORAGE_128,
                STORAGE_256,
                STORAGE_512,
                STORAGE_1024,
            };


            // Seed skus
            List<SkuPriceId> skus = new List<SkuPriceId>();
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb0"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb1"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb2"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb3"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb4"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb5"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb6"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb7"));


            // Seed sku prices
            List<SkuPriceList> prices = new List<SkuPriceList>();

            // Model 0 (IPHONE_16_E), Color 0 (WHITE_16_E)
            prices.Add(SkuPriceList.Create(skus[0].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[1].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[2].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1500));
            prices.Add(SkuPriceList.Create(skus[3].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1600));

            // Model 0 (IPHONE_16_E), Color 1 (BLACK_16_E)
            prices.Add(SkuPriceList.Create(skus[4].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[5].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[6].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1500));
            prices.Add(SkuPriceList.Create(skus[7].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1600));

            // Seed iPhone category
            Category IPHONE_CATEGORY = Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null, parentCategory: null);

            // Seed iPhone 16 E model
            var IPHONE_16_E_MODEL = IphoneModel.Create(iPhoneModelId: ModelId.Of("6643543e0087aa09993f5b15"),
                                              category: IPHONE_CATEGORY,
                                              name: "iPhone 16 E",
                                              models: new List<Model>
                                              {
                                                    IPHONE_16_E
                                              },
                                              colors: new List<Color>
                                              {
                                                    WHITE_16_E,
                                                    BLACK_16_E
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
                                              description: "iPhone 16 E model description.",
                                              averageRating: initAverageRating,
                                              ratingStars: initRatingStars,
                                              isNewest: false);

            return new List<IphoneModel>
            {
                IPHONE_16_E_MODEL
            };
        }
    }

    public static IEnumerable<IphoneModel> Iphone17Models
    {
        get
        {
            Model IPHONE_17 = Model.Create("iPhone 17", 0);



            Color LAVENDER_17 = Color.Create("Lavender", "#E7D9F2", "iphone-17-finish-select-202509-lavender_ttymfa", 0);
            Color SAGE_17 = Color.Create("Sage", "#BBC89E", "iphone-17-finish-select-202509-sage_aw371h", 1);
            Color MIST_BLUE_17 = Color.Create("Mist Blue", "#A7BDDE", "iphone-17-finish-select-202509-mistblue_gcqb5o", 2);
            Color WHITE_17 = Color.Create("White", "#FAFAFA", "iphone-17-finish-select-202509-white_hphgpt", 3);
            Color BLACK_17 = Color.Create("Black", "#4B4F50", "iphone-17-finish-select-202509-black_df2lsp", 4);

            // Seed storages
            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            // Seed showcase images
            List<Image> showcaseImages = new List<Image>
            {
                Image.Create("iphone-17-finish-select-202509-lavender_ttymfa", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-17-finish-select-202509-lavender_ttymfa.webp", "", "", 0, 0, 0, 0),
                Image.Create("iphone-17-finish-select-202509-sage_aw371h", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-17-finish-select-202509-sage_aw371h.webp", "", "", 0, 0, 0, 1),
                Image.Create("iphone-17-finish-select-202509-mistblue_gcqb5o", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-17-finish-select-202509-mistblue_gcqb5o.webp", "", "", 0, 0, 0, 2),
                Image.Create("iphone-17-finish-select-202509-white_hphgpt", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-17-finish-select-202509-white_hphgpt.webp", "", "", 0, 0, 0, 3),
                Image.Create("iphone-17-finish-select-202509-black_df2lsp", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-17-finish-select-202509-black_df2lsp.webp", "", "", 0, 0, 0, 4),
            };


            // Seed average rating and rating stars
            AverageRating initAverageRating = AverageRating.Create(0, 0);
            List<RatingStar> initRatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };

            // Seed models in prices
            List<Model> modelsInPrices = new()
            {
                IPHONE_17,
            };

            // Seed colors in prices
            List<Color> colorsInPrices = new()
            {
                LAVENDER_17,
                SAGE_17,
                MIST_BLUE_17,
                WHITE_17,
                BLACK_17,
            };

            // Seed storages in prices
            List<Storage> storagesInPrices = new()
            {
                STORAGE_128,
                STORAGE_256,
                STORAGE_512,
                STORAGE_1024,
            };

            // Seed skus
            List<SkuPriceId> skus = new List<SkuPriceId>();
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb8"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fb9"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fba"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fbb"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fbc"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fbd"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fbe"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fbf"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc0"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc1"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc2"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc3"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc4"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc5"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc6"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc7"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc8"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fc9"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fca"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fcb"));


            // Seed sku prices
            List<SkuPriceList> prices = new List<SkuPriceList>();

            // Model 0 (IPHONE_17), Color 0 (LAVENDER_17)
            prices.Add(SkuPriceList.Create(skus[0].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1500));
            prices.Add(SkuPriceList.Create(skus[1].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1600));
            prices.Add(SkuPriceList.Create(skus[2].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1700));
            prices.Add(SkuPriceList.Create(skus[3].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1800));

            // Model 0 (IPHONE_17), Color 1 (SAGE_17)
            prices.Add(SkuPriceList.Create(skus[4].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1500));
            prices.Add(SkuPriceList.Create(skus[5].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1600));
            prices.Add(SkuPriceList.Create(skus[6].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1700));
            prices.Add(SkuPriceList.Create(skus[7].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1800));

            // Model 0 (IPHONE_17), Color 2 (MIST_BLUE_17)
            prices.Add(SkuPriceList.Create(skus[8].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1500));
            prices.Add(SkuPriceList.Create(skus[9].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1600));
            prices.Add(SkuPriceList.Create(skus[10].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1700));
            prices.Add(SkuPriceList.Create(skus[11].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1800));

            // Model 0 (IPHONE_17), Color 3 (WHITE_17)
            prices.Add(SkuPriceList.Create(skus[12].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1500));
            prices.Add(SkuPriceList.Create(skus[13].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1600));
            prices.Add(SkuPriceList.Create(skus[14].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1700));
            prices.Add(SkuPriceList.Create(skus[15].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1800));

            // Model 0 (IPHONE_17), Color 4 (BLACK_17)
            prices.Add(SkuPriceList.Create(skus[16].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1500));
            prices.Add(SkuPriceList.Create(skus[17].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1600));
            prices.Add(SkuPriceList.Create(skus[18].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1700));
            prices.Add(SkuPriceList.Create(skus[19].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1800));

            // Seed iPhone category
            Category IPHONE_CATEGORY = Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null, parentCategory: null);

            // Seed iPhone 17 model
            var IPHONE_17_MODEL = IphoneModel.Create(iPhoneModelId: ModelId.Of("6643543f0087aa09993f5b16"),
                                              category: IPHONE_CATEGORY,
                                              name: "iPhone 17",
                                              models: new List<Model>
                                              {
                                                    IPHONE_17
                                              },
                                              colors: new List<Color>
                                              {
                                                    LAVENDER_17,
                                                    SAGE_17,
                                                    MIST_BLUE_17,
                                                    WHITE_17,
                                                    BLACK_17
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
                                              description: "iPhone 17 model description.",
                                              averageRating: initAverageRating,
                                              ratingStars: initRatingStars,
                                              isNewest: true);

            return new List<IphoneModel>
            {
                IPHONE_17_MODEL
            };
        }
    }

    public static IEnumerable<IphoneModel> Iphone17ProModels
    {
        get
        {
            Model IPHONE_17_PRO = Model.Create("iPhone 17 Pro", 0);
            Model IPHONE_17_PRO_MAX = Model.Create("iPhone 17 Pro Max", 1);


            Color SILVER_17_PRO = Color.Create("Silver", "#E6E6E6", "iphone-17-pro-finish-select-202509-6-3inch-silver_p9eegd", 0);
            Color COSMIC_ORANGE_17_PRO = Color.Create("Cosmic Orange", "#F67E36", "iphone-17-pro-finish-select-202509-6-3inch-cosmicorange_ye9ms2", 1);
            Color DEEP_BLUE_17_PRO = Color.Create("Deep Blue", "#3F4C77", "iphone-17-pro-finish-select-202509-6-3inch-deepblue_xhdpyx", 2);

            // Seed storages
            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            // Seed showcase images
            List<Image> showcaseImages = new List<Image>
            {
                Image.Create("iphone-17-pro-finish-select-202509-6-3inch-silver_p9eegd", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-17-pro-finish-select-202509-6-3inch-silver_p9eegd.webp", "", "", 0, 0, 0, 0),
                Image.Create("iphone-17-pro-finish-select-202509-6-3inch-cosmicorange_ye9ms2", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-17-pro-finish-select-202509-6-3inch-cosmicorange_ye9ms2.webp", "", "", 0, 0, 0, 1),
                Image.Create("iphone-17-pro-finish-select-202509-6-3inch-deepblue_xhdpyx", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-17-pro-finish-select-202509-6-3inch-deepblue_xhdpyx.webp", "", "", 0, 0, 0, 2),
            };


            // Seed average rating and rating stars
            AverageRating initAverageRating = AverageRating.Create(0, 0);
            List<RatingStar> initRatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };

            // Seed models in prices
            List<Model> modelsInPrices = new()
            {
                IPHONE_17_PRO,
                IPHONE_17_PRO_MAX,
            };

            // Seed colors in prices
            List<Color> colorsInPrices = new()
            {
                SILVER_17_PRO,
                COSMIC_ORANGE_17_PRO,
                DEEP_BLUE_17_PRO,
            };

            // Seed storages in prices
            List<Storage> storagesInPrices = new()
            {
                STORAGE_128,
                STORAGE_256,
                STORAGE_512,
                STORAGE_1024,
            };

            // Seed skus
            List<SkuPriceId> skus = new List<SkuPriceId>();
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fcc"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fcd"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fce"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fcf"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd0"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd1"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd2"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd3"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd4"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd5"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd6"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd7"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd8"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fd9"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fda"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fdb"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fdc"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fdd"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fde"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fdf"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe0"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe1"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe2"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe3"));

            // Seed sku prices
            List<SkuPriceList> prices = new List<SkuPriceList>();

            // Model 0 (IPHONE_17_PRO), Color 0 (SILVER_17_PRO)
            prices.Add(SkuPriceList.Create(skus[0].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1800));
            prices.Add(SkuPriceList.Create(skus[1].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1900));
            prices.Add(SkuPriceList.Create(skus[2].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 2000));
            prices.Add(SkuPriceList.Create(skus[3].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 2100));

            // Model 0 (IPHONE_17_PRO), Color 1 (COSMIC_ORANGE_17_PRO)
            prices.Add(SkuPriceList.Create(skus[4].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1800));
            prices.Add(SkuPriceList.Create(skus[5].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1900));
            prices.Add(SkuPriceList.Create(skus[6].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 2000));
            prices.Add(SkuPriceList.Create(skus[7].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 2100));

            // Model 0 (IPHONE_17_PRO), Color 2 (DEEP_BLUE_17_PRO)
            prices.Add(SkuPriceList.Create(skus[8].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1800));
            prices.Add(SkuPriceList.Create(skus[9].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1900));
            prices.Add(SkuPriceList.Create(skus[10].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 2000));
            prices.Add(SkuPriceList.Create(skus[11].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 2100));

            // Model 1 (IPHONE_17_PRO_MAX), Color 0 (SILVER_17_PRO)
            prices.Add(SkuPriceList.Create(skus[12].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1800));
            prices.Add(SkuPriceList.Create(skus[13].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1900));
            prices.Add(SkuPriceList.Create(skus[14].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 2000));
            prices.Add(SkuPriceList.Create(skus[15].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 2100));

            // Model 1 (IPHONE_17_PRO_MAX), Color 1 (COSMIC_ORANGE_17_PRO)
            prices.Add(SkuPriceList.Create(skus[16].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1800));
            prices.Add(SkuPriceList.Create(skus[17].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1900));
            prices.Add(SkuPriceList.Create(skus[18].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 2000));
            prices.Add(SkuPriceList.Create(skus[19].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 2100));

            // Model 1 (IPHONE_17_PRO_MAX), Color 2 (DEEP_BLUE_17_PRO)
            prices.Add(SkuPriceList.Create(skus[20].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1800));
            prices.Add(SkuPriceList.Create(skus[21].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1900));
            prices.Add(SkuPriceList.Create(skus[22].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 2000));
            prices.Add(SkuPriceList.Create(skus[23].Value!, modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 2100));

            // Seed iPhone category
            Category IPHONE_CATEGORY = Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null, parentCategory: null);

            // Seed iPhone 17 Pro model
            var IPHONE_17_PRO_MODEL = IphoneModel.Create(iPhoneModelId: ModelId.Of("664354400087aa09993f5b17"),
                                              category: IPHONE_CATEGORY,
                                              name: "iPhone 17 Pro",
                                              models: new List<Model>
                                              {
                                                    IPHONE_17_PRO,
                                                    IPHONE_17_PRO_MAX
                                              },
                                              colors: new List<Color>
                                              {
                                                    SILVER_17_PRO,
                                                    COSMIC_ORANGE_17_PRO,
                                                    DEEP_BLUE_17_PRO
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
                                              description: "iPhone 17 Pro model description.",
                                              averageRating: initAverageRating,
                                              ratingStars: initRatingStars,
                                              isNewest: true);

            return new List<IphoneModel>
            {
                IPHONE_17_PRO_MODEL
            };
        }
    }

    public static IEnumerable<IphoneModel> Iphone17AirModels
    {
        get
        {
            Model IPHONE_17_AIR = Model.Create("iPhone 17 Air", 0);


            // Seed colors
            Color SKY_BLUE_17_AIR = Color.Create("Sky Blue", "#E5F2FA", "iphone-air-finish-select-202509-skyblue_hhd2cg", 0);
            Color LIGHT_GOLD_17_AIR = Color.Create("Light Gold", "#FAF3E6", "iphone-air-finish-select-202509-lightgold_zegr3u", 1);
            Color CLOUD_WHITE_17_AIR = Color.Create("Cloud White", "#FCFCFC", "iphone-air-finish-select-202509-cloudwhite_ubwvpe", 2);
            Color SPACE_BLACK_17_AIR = Color.Create("Space Black", "#121212", "iphone-air-finish-select-202509-spaceblack_xtp6d4", 3);

            // Seed storages
            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            // Seed showcase images
            List<Image> showcaseImages = new List<Image>
            {
                Image.Create("iphone-air-finish-select-202509-skyblue_hhd2cg", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-air-finish-select-202509-skyblue_hhd2cg.webp", "", "", 0, 0, 0, 0),
                Image.Create("iphone-air-finish-select-202509-lightgold_zegr3u", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-air-finish-select-202509-lightgold_zegr3u.webp", "", "", 0, 0, 0, 1),
                Image.Create("iphone-air-finish-select-202509-cloudwhite_ubwvpe", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-air-finish-select-202509-cloudwhite_ubwvpe.webp", "", "", 0, 0, 0, 2),
                Image.Create("iphone-air-finish-select-202509-spaceblack_xtp6d4", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-air-finish-select-202509-spaceblack_xtp6d4.webp", "", "", 0, 0, 0, 3),
            };


            // Seed average rating and rating stars
            AverageRating initAverageRating = AverageRating.Create(0, 0);
            List<RatingStar> initRatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };

            // Seed models in prices
            List<Model> modelsInPrices = new()
            {
                IPHONE_17_AIR,
            };

            // Seed colors in prices
            List<Color> colorsInPrices = new()
            {
                SKY_BLUE_17_AIR,
                LIGHT_GOLD_17_AIR,
                CLOUD_WHITE_17_AIR,
                SPACE_BLACK_17_AIR,
            };

            // Seed storages in prices
            List<Storage> storagesInPrices = new()
            {
                STORAGE_128,
                STORAGE_256,
                STORAGE_512,
                STORAGE_1024,
            };

            // Seed skus
            List<SkuPriceId> skus = new List<SkuPriceId>();
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe4"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe5"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe6"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe7"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe8"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fe9"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fea"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23feb"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fec"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fed"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fee"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23fef"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23ff0"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23ff1"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23ff2"));
            skus.Add(SkuPriceId.Of("690f4601e2295b9f94f23ff3"));

            // Seed sku prices
            List<SkuPriceList> prices = new List<SkuPriceList>();

            // Model 0 (IPHONE_17_AIR), Color 0 (SKY_BLUE_17_AIR)
            prices.Add(SkuPriceList.Create(skus[0].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[1].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[2].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[3].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 0 (IPHONE_17_AIR), Color 1 (LIGHT_GOLD_17_AIR)
            prices.Add(SkuPriceList.Create(skus[4].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[5].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[6].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[7].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 0 (IPHONE_17_AIR), Color 2 (CLOUD_WHITE_17_AIR)
            prices.Add(SkuPriceList.Create(skus[8].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[9].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[10].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[11].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Model 0 (IPHONE_17_AIR), Color 3 (SPACE_BLACK_17_AIR)
            prices.Add(SkuPriceList.Create(skus[12].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1200));
            prices.Add(SkuPriceList.Create(skus[13].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1300));
            prices.Add(SkuPriceList.Create(skus[14].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1400));
            prices.Add(SkuPriceList.Create(skus[15].Value!, modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1500));

            // Seed iPhone category
            Category IPHONE_CATEGORY = Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null, parentCategory: null);

            // Seed iPhone 17 Air model
            var IPHONE_17_AIR_MODEL = IphoneModel.Create(iPhoneModelId: ModelId.Of("664354410087aa09993f5b18"),
                                              category: IPHONE_CATEGORY,
                                              name: "iPhone 17 Air",
                                              models: new List<Model>
                                              {
                                                    IPHONE_17_AIR
                                              },
                                              colors: new List<Color>
                                              {
                                                    SKY_BLUE_17_AIR,
                                                    LIGHT_GOLD_17_AIR,
                                                    CLOUD_WHITE_17_AIR,
                                                    SPACE_BLACK_17_AIR
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
                                              description: "iPhone 17 Air model description.",
                                              averageRating: initAverageRating,
                                              ratingStars: initRatingStars,
                                              isNewest: true);

            return new List<IphoneModel>
            {
                IPHONE_17_AIR_MODEL
            };
        }
    }
}
