
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public partial class Seeds
{
    public static IEnumerable<IPhone16Model> IPhone15_15Plus_Models
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

            var modelList1 = new List<ModelBK>
            {
                ModelBK.Create(modelName: "iPhone 15",
                             modelOrder: 0),
                ModelBK.Create(modelName: "iPhone 15 Plus",
                             modelOrder: 1),
            };

            var colorList1 = new List<ColorBK>
            {
                ColorBK.Create(colorName: "blue",
                             colorHex: "#D5DDDF",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-blue-202309",
                             colorOrder: 0),
                ColorBK.Create(colorName: "pink",
                             colorHex: "#EBD3D4",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-pink-202309",
                             colorOrder: 1),
                ColorBK.Create(colorName: "yellow",
                             colorHex: "#EDE6C8",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-yellow-202309",
                             colorOrder: 2),
                ColorBK.Create(colorName: "green",
                             colorHex: "#D0D9CA",
                             colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-green-202309",
                             colorOrder: 3),
                ColorBK.Create(colorName: "black",
                             colorHex: "#4B4F50",
                             colorImage:"https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202309",
                             colorOrder: 4),
            };

            var imageList1 = new List<Image>
            {
                Image.Create(imageId: "image_id_1",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
                Image.Create(imageId: "image_id_5",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 4),
            };

            var ip_15_model = IPhone16Model.Create(iPhone16ModelId: IPhone16ModelId.Of("67c1871284c95016a7573f10"),
                                                   name: "iPhone 15",
                                                   models: modelList1,
                                                   colors: colorList1,
                                                   storages: storageList,
                                                   description: "",
                                                   averageRating: AverageRating.Create(0, 0),
                                                   ratingStars: ratingInit,
                                                   descriptionImages: imageList1,
                                                   categoryId: CategoryId.Of("92dc470aa9ee0a5e6fbafdbd"));

            return new List<IPhone16Model>
            {
                ip_15_model
            };
        }
    }
}
