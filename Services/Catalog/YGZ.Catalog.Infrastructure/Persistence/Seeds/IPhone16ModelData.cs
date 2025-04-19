

using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public partial class Seeds
{
    public static IEnumerable<IPhone16Model> IPhone16_16Plus_Models
    {
        get
        {
            var ratingInit = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            };

            var storageList = new List<Storage>
            {
                Storage.STORAGE_128,
                Storage.STORAGE_256,
                Storage.STORAGE_512,
                Storage.STORAGE_1024,
            };

            var modelList1 = new List<Model>
            {
                Model.Create(modelName: "iPhone 16",
                             modelOrder: 0),
                Model.Create(modelName: "iPhone 16 Plus",
                             modelOrder: 1),
            };

            var colorList1 = new List<Color>
            {
                Color.Create(colorName: "ultramarine",
                             colorHex: "#9AADF6",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-ultramarine-202409",
                             colorOrder: 0),
                Color.Create(colorName: "teal",
                             colorHex: "#B0D4D2",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-teal-202409",
                             colorOrder: 1),
                Color.Create(colorName: "pink",
                             colorHex: "#F2ADDA",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-pink-202409",
                             colorOrder: 2),
                Color.Create(colorName: "white",
                             colorHex: "#FAFAFA",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-white-202409",
                             colorOrder: 3),
                Color.Create(colorName: "black",
                             colorHex: "#3C4042",
                             colorImage:"https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202409",
                             colorOrder: 4),
            };

            var imageList1 = new List<Image>
            {
                Image.Create(imageId: "image_id_1",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811098/iphone-16-finish-select-202409-6-1inch-pink_q2saue.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811404/iphone-16-finish-select-202409-6-1inch-teal_gfumfa.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811448/iphone-16-finish-select-202409-6-1inch-white_ghumel.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
                Image.Create(imageId: "image_id_5",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811496/iphone-16-finish-select-202409-6-1inch-black_wnfzl5.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 4),
            };

            var ip_16 = IPhone16Model.Create(iPhone16ModelId: IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                                             name: "iPhone 16",
                                             models: modelList1,
                                             colors: colorList1,
                                             storages: storageList,
                                             description: "",
                                             averageRating: AverageRating.Create(0, 0),
                                             ratingStars: ratingInit,
                                             descriptionImages: imageList1,
                                             categoryId: CategoryId.Of("91dc470aa9ee0a5e6fbafdbc"));

            return new List<IPhone16Model>
            {
                ip_16
            };
        }
    }
}
