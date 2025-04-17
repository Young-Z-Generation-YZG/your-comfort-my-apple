using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence;

public class SeedData
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
                Model.Create(modelName: "iPhone 16 PLus",
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
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811462/iphone-16-finish-select-202409-6-1inch-white_AV1_qnax3q.webp",
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

    public static IEnumerable<IPhone16Model> IPhone16e_Models
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
                Model.Create(modelName: "iPhone 16e",
                             modelOrder: 0),
            };

            var colorList1 = new List<Color>
            {
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
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744701741/iphone-16e-finish-select-202502-white_g1coja.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744702305/iphone-16e-finish-select-202502-black_yq48ki.webp",
                             imageName: "6ip",
                             imageDescription: "6 ip in an images",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1)
            };

            var ip_16e = IPhone16Model.Create(iPhone16ModelId: IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                                             name: "iPhone 16e",
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
                ip_16e
            };
        }
    }

    public static IEnumerable<IPhone16Detail> IPhone16_16Plus_Details
    {
        get
        {
            var color_ultramarine = Color.Create("ultramarine", "#9AADF6", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-ultramarine-202409", null);
            var color_teal = Color.Create("teal", "#B0D4D2", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-teal-202409", null);
            var color_pink = Color.Create("pink", "#F2ADDA", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-pink-202409", null);
            var color_white = Color.Create("white", "#FAFAFA", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-white-202409", null);
            var color_black = Color.Create("black", "#3C4042", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202409", null);

            var imageList_ultramarine = new List<Image>
            {
                Image.Create(imageId: "image_id_1",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV1",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV2",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV3",
                             imageName: "iPhone 16",
                             imageDescription: "Place four",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
            };
            var imageList_teal = new List<Image>
            {
                  Image.Create(imageId: "image_id_1",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-teal",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-teal_AV1",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-teal_AV2",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-teal_AV3",
                             imageName: "iPhone 16",
                             imageDescription: "Place four",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
            };
            var imageList_pink = new List<Image>
            {
                  Image.Create(imageId: "image_id_1",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-pink",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-pink_AV1",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-pink_AV2",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-pink_AV3",
                             imageName: "iPhone 16",
                             imageDescription: "Place four",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
            };
            var imageList_white = new List<Image>
            {
                  Image.Create(imageId: "image_id_1",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-white",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-white_AV1",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-white_AV2",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-white_AV3",
                             imageName: "iPhone 16",
                             imageDescription: "Place four",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
            };
            var imageList_black = new List<Image>
            {
                  Image.Create(imageId: "image_id_1",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-black",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-black_AV1",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-black_AV2",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-black_AV3",
                             imageName: "iPhone 16",
                             imageDescription: "Place four",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
            };

            var ip_16_ultramarine_128 = new IPhone16Detail(IPhone16Id.Of("67cbcff3cb422bbaf809c5a9"))
            {
                Model = "iPhone 16",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_128,
                UnitPrice = 699,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_ultramarine_256 = new IPhone16Detail(IPhone16Id.Of("67dbcd41a9ee0a5e6fbafd77"))
            {
                Model = "iPhone 16",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_256,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_ultramarine_512 = new IPhone16Detail(IPhone16Id.Of("67dbcd4aa9ee0a5e6fbafd78"))
            {
                Model = "iPhone 16",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_512,
                UnitPrice = 999,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_ultramarine_1024 = new IPhone16Detail(IPhone16Id.Of("67dbcd52a9ee0a5e6fbafd79"))
            {
                Model = "iPhone 16",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1199,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_teal_128 = new IPhone16Detail(IPhone16Id.Of("67dbcd6da9ee0a5e6fbafd7a"))
            {
                Model = "iPhone 16",
                Color = color_teal,
                Storage = Storage.STORAGE_128,
                UnitPrice = 699,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_teal_256 = new IPhone16Detail(IPhone16Id.Of("67dbcdb8a9ee0a5e6fbafd7f"))
            {
                Model = "iPhone 16",
                Color = color_teal,
                Storage = Storage.STORAGE_256,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_teal_512 = new IPhone16Detail(IPhone16Id.Of("67dbcd8ca9ee0a5e6fbafd7c"))
            {
                Model = "iPhone 16",
                Color = color_teal,
                Storage = Storage.STORAGE_512,
                UnitPrice = 999,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_teal_1024 = new IPhone16Detail(IPhone16Id.Of("67dbcdc8a9ee0a5e6fbafd80"))
            {
                Model = "iPhone 16",
                Color = color_teal,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1199,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_pink_128 = new IPhone16Detail(IPhone16Id.Of("67dbcdaaa9ee0a5e6fbafd7e"))
            {
                Model = "iPhone 16",
                Color = color_pink,
                Storage = Storage.STORAGE_128,
                UnitPrice = 699,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_pink_256 = new IPhone16Detail(IPhone16Id.Of("67dbcde6a9ee0a5e6fbafd81"))
            {
                Model = "iPhone 16",
                Color = color_pink,
                Storage = Storage.STORAGE_256,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_pink_512 = new IPhone16Detail(IPhone16Id.Of("67dbcdf8a9ee0a5e6fbafd82"))
            {
                Model = "iPhone 16",
                Color = color_pink,
                Storage = Storage.STORAGE_512,
                UnitPrice = 999,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_pink_1024 = new IPhone16Detail(IPhone16Id.Of("67dbcdfaa9ee0a5e6fbafd83"))
            {
                Model = "iPhone 16",
                Color = color_pink,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1199,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_white_128 = new IPhone16Detail(IPhone16Id.Of("67dbcdfba9ee0a5e6fbafd84"))
            {
                Model = "iPhone 16",
                Color = color_white,
                Storage = Storage.STORAGE_128,
                UnitPrice = 699,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_white_256 = new IPhone16Detail(IPhone16Id.Of("67dbcdfda9ee0a5e6fbafd85"))
            {
                Model = "iPhone 16",
                Color = color_white,
                Storage = Storage.STORAGE_256,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_white_512 = new IPhone16Detail(IPhone16Id.Of("67dbcdfea9ee0a5e6fbafd86"))
            {
                Model = "iPhone 16",
                Color = color_white,
                Storage = Storage.STORAGE_512,
                UnitPrice = 999,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_white_1024 = new IPhone16Detail(IPhone16Id.Of("67dbcdffa9ee0a5e6fbafd87"))
            {
                Model = "iPhone 16",
                Color = color_white,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1199,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_black_128 = new IPhone16Detail(IPhone16Id.Of("67dbce01a9ee0a5e6fbafd88"))
            {
                Model = "iPhone 16",
                Color = color_black,
                Storage = Storage.STORAGE_128,
                UnitPrice = 699,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_black_256 = new IPhone16Detail(IPhone16Id.Of("67dbce02a9ee0a5e6fbafd89"))
            {
                Model = "iPhone 16",
                Color = color_black,
                Storage = Storage.STORAGE_256,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_black_512 = new IPhone16Detail(IPhone16Id.Of("67dbce04a9ee0a5e6fbafd8a"))
            {
                Model = "iPhone 16",
                Color = color_black,
                Storage = Storage.STORAGE_512,
                UnitPrice = 999,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_black_1024 = new IPhone16Detail(IPhone16Id.Of("67dbce05a9ee0a5e6fbafd8b"))
            {
                Model = "iPhone 16",
                Color = color_black,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1199,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_plus_ultramarine_128 = new IPhone16Detail(IPhone16Id.Of("67dbce06a9ee0a5e6fbafd8c"))
            {
                Model = "iPhone 16 Plus",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_128,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_ultramarine_256 = new IPhone16Detail(IPhone16Id.Of("67dbce07a9ee0a5e6fbafd8d"))
            {
                Model = "iPhone 16 Plus",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_256,
                UnitPrice = 899,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_ultramarine_512 = new IPhone16Detail(IPhone16Id.Of("67dbce08a9ee0a5e6fbafd8e"))
            {
                Model = "iPhone 16 Plus",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_512,
                UnitPrice = 1099,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_ultramarine_1024 = new IPhone16Detail(IPhone16Id.Of("67dbce09a9ee0a5e6fbafd8f"))
            {
                Model = "iPhone 16 Plus",
                Color = color_ultramarine,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1299,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_ultramarine,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_plus_teal_128 = new IPhone16Detail(IPhone16Id.Of("67dbce0aa9ee0a5e6fbafd90"))
            {
                Model = "iPhone 16 Plus",
                Color = color_teal,
                Storage = Storage.STORAGE_128,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_teal_256 = new IPhone16Detail(IPhone16Id.Of("67dbce0ba9ee0a5e6fbafd91"))
            {
                Model = "iPhone 16 Plus",
                Color = color_teal,
                Storage = Storage.STORAGE_256,
                UnitPrice = 899,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_teal_512 = new IPhone16Detail(IPhone16Id.Of("67dbce0ca9ee0a5e6fbafd92"))
            {
                Model = "iPhone 16 Plus",
                Color = color_teal,
                Storage = Storage.STORAGE_512,
                UnitPrice = 1099,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_teal_1024 = new IPhone16Detail(IPhone16Id.Of("67dbce0da9ee0a5e6fbafd93"))
            {
                Model = "iPhone 16 Plus",
                Color = color_teal,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1299,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_teal,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_plus_pink_128 = new IPhone16Detail(IPhone16Id.Of("67dbce0ea9ee0a5e6fbafd94"))
            {
                Model = "iPhone 16 Plus",
                Color = color_pink,
                Storage = Storage.STORAGE_128,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_pink_256 = new IPhone16Detail(IPhone16Id.Of("67dbce0fa9ee0a5e6fbafd95"))
            {
                Model = "iPhone 16 Plus",
                Color = color_pink,
                Storage = Storage.STORAGE_256,
                UnitPrice = 899,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_pink_512 = new IPhone16Detail(IPhone16Id.Of("67dbce10a9ee0a5e6fbafd96"))
            {
                Model = "iPhone 16 Plus",
                Color = color_pink,
                Storage = Storage.STORAGE_512,
                UnitPrice = 1099,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_pink_1024 = new IPhone16Detail(IPhone16Id.Of("67dbce11a9ee0a5e6fbafd97"))
            {
                Model = "iPhone 16 Plus",
                Color = color_pink,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1299,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_pink,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_plus_white_128 = new IPhone16Detail(IPhone16Id.Of("67dbce12a9ee0a5e6fbafd98"))
            {
                Model = "iPhone 16 Plus",
                Color = color_white,
                Storage = Storage.STORAGE_128,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_white_256 = new IPhone16Detail(IPhone16Id.Of("67dbce13a9ee0a5e6fbafd99"))
            {
                Model = "iPhone 16 Plus",
                Color = color_white,
                Storage = Storage.STORAGE_256,
                UnitPrice = 899,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_white_512 = new IPhone16Detail(IPhone16Id.Of("67dbce14a9ee0a5e6fbafd9a"))
            {
                Model = "iPhone 16 Plus",
                Color = color_white,
                Storage = Storage.STORAGE_512,
                UnitPrice = 1099,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_white_1024 = new IPhone16Detail(IPhone16Id.Of("67dbce15a9ee0a5e6fbafd9b"))
            {
                Model = "iPhone 16 Plus",
                Color = color_white,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1299,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16_plus_black_128 = new IPhone16Detail(IPhone16Id.Of("67dbce16a9ee0a5e6fbafd9c"))
            {
                Model = "iPhone 16 Plus",
                Color = color_black,
                Storage = Storage.STORAGE_128,
                UnitPrice = 799,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_black_256 = new IPhone16Detail(IPhone16Id.Of("67dbce17a9ee0a5e6fbafd9d"))
            {
                Model = "iPhone 16 Plus",
                Color = color_black,
                Storage = Storage.STORAGE_256,
                UnitPrice = 899,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_black_512 = new IPhone16Detail(IPhone16Id.Of("67dbce18a9ee0a5e6fbafd9e"))
            {
                Model = "iPhone 16 Plus",
                Color = color_black,
                Storage = Storage.STORAGE_512,
                UnitPrice = 1099,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16_plus_black_1024 = new IPhone16Detail(IPhone16Id.Of("67dbce19a9ee0a5e6fbafd9f"))
            {
                Model = "iPhone 16 Plus",
                Color = color_black,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1299,
                GeneralModel = "iphone-16",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16 plus {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            return new List<IPhone16Detail>
            {
                ip_16_ultramarine_128,
                ip_16_ultramarine_256,
                ip_16_ultramarine_512,
                ip_16_ultramarine_1024,
                ip_16_teal_128,
                ip_16_teal_256,
                ip_16_teal_512,
                ip_16_teal_1024,
                ip_16_pink_128,
                ip_16_pink_256,
                ip_16_pink_512,
                ip_16_pink_1024,
                ip_16_white_128,
                ip_16_white_256,
                ip_16_white_512,
                ip_16_white_1024,
                ip_16_black_128,
                ip_16_black_256,
                ip_16_black_512,
                ip_16_black_1024,
                ip_16_plus_ultramarine_128,
                ip_16_plus_ultramarine_256,
                ip_16_plus_ultramarine_512,
                ip_16_plus_ultramarine_1024,
                ip_16_plus_teal_128,
                ip_16_plus_teal_256,
                ip_16_plus_teal_512,
                ip_16_plus_teal_1024,
                ip_16_plus_pink_128,
                ip_16_plus_pink_256,
                ip_16_plus_pink_512,
                ip_16_plus_pink_1024,
                ip_16_plus_white_128,
                ip_16_plus_white_256,
                ip_16_plus_white_512,
                ip_16_plus_white_1024,
                ip_16_plus_black_128,
                ip_16_plus_black_256,
                ip_16_plus_black_512,
                ip_16_plus_black_1024
            };
        }
    }

    public static IEnumerable<IPhone16Detail> IPhone16e_Details
    {
        get
        {
            var color_white = Color.Create("white", "#FAFAFA", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-white-202409", null);
            var color_black = Color.Create("black", "#3C4042", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202409", null);

            var imageList_white = new List<Image>
            {
                Image.Create(imageId: "image_id_1",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744701741/iphone-16e-finish-select-202502-white_g1coja.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744701785/iphone-16e-finish-select-202502-white_AV1_rgvmuo.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744701798/iphone-16e-finish-select-202502-white_AV2_pgjbeu.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
            };
            var imageList_black = new List<Image>
            {
                  Image.Create(imageId: "image_id_1",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744702305/iphone-16e-finish-select-202502-black_yq48ki.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744702319/iphone-16e-finish-select-202502-black_AV1_g1pnoq.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744702342/iphone-16e-finish-select-202502-black_AV2_oijb4q.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
            };

            var ip_16e_white_128 = new IPhone16Detail(IPhone16Id.Of("67fdc6d448b39da4de12f69a"))
            {
                Model = "iPhone 16e",
                Color = color_white,
                Storage = Storage.STORAGE_128,
                UnitPrice = 799,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16e_white_256 = new IPhone16Detail(IPhone16Id.Of("67fdcdb448b39da4de12f6bf"))
            {
                Model = "iPhone 16e",
                Color = color_white,
                Storage = Storage.STORAGE_256,
                UnitPrice = 899,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16e_white_512 = new IPhone16Detail(IPhone16Id.Of("66434c34fd0d76dfa0eee6d8"))
            {
                Model = "iPhone 16e",
                Color = color_white,
                Storage = Storage.STORAGE_512,
                UnitPrice = 1099,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16e_white_1024 = new IPhone16Detail(IPhone16Id.Of("66434d1afd0d76dfa0eee8af"))
            {
                Model = "iPhone 16e",
                Color = color_white,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1299,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_white,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            var ip_16e_black_128 = new IPhone16Detail(IPhone16Id.Of("664c021b529494b708d7ae89"))
            {
                Model = "iPhone 16e",
                Color = color_black,
                Storage = Storage.STORAGE_128,
                UnitPrice = 799,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_128.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16e_black_256 = new IPhone16Detail(IPhone16Id.Of("664c024f529494b708d7aeb0"))
            {
                Model = "iPhone 16e",
                Color = color_black,
                Storage = Storage.STORAGE_256,
                UnitPrice = 899,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_256.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16e_black_512 = new IPhone16Detail(IPhone16Id.Of("664439317954a1ae3c523650"))
            {
                Model = "iPhone 16e",
                Color = color_black,
                Storage = Storage.STORAGE_512,
                UnitPrice = 1099,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_512.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };
            var ip_16e_black_1024 = new IPhone16Detail(IPhone16Id.Of("6645cf80af463a6f3221b483"))
            {
                Model = "iPhone 16e",
                Color = color_black,
                Storage = Storage.STORAGE_1024,
                UnitPrice = 1299,
                GeneralModel = "iphone-16e",
                Description = "The iPhone 16 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
                AvailableInStock = 100,
                TotalSold = 0,
                State = State.ACTIVE,
                Images = imageList_black,
                Slug = Slug.Create($"iphone 16e {Storage.STORAGE_1024.Name}"),
                IPhoneModelId = IPhone16ModelId.Of("664439317954a1ae3c52364c"),
                CategoryId = CategoryId.Of("91dc470aa9ee0a5e6fbafdbc")
            };

            return new List<IPhone16Detail>
            {
                ip_16e_white_128,
                ip_16e_white_256,
                ip_16e_white_512,
                ip_16e_white_1024,
                ip_16e_black_128,
                ip_16e_black_256,
                ip_16e_black_512,
                ip_16e_black_1024
            };
        }
    }

    public static IEnumerable<Category> Categories => new List<Category>
    {
        Category.Create(id: CategoryId.Of("67dc43ee9b19c6773e9cec55"), name: "Mac", description: "Mac categories.", order: 0, parentId: null),
        Category.Create(id: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa"), name: "iPad", description: "iPad categories.", order: 1, parentId: null),
        Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null),
        Category.Create(id: CategoryId.Of("67dc470ca9ee0a5e6fbafdac"), name: "Watch", description: "Watch categories.", order: 3, parentId: null),
        Category.Create(id: CategoryId.Of("67dc470ea9ee0a5e6fbafdad"), name: "HeadPhones", description: "HeadPhones categories.", order: 4, parentId: null),

        Category.Create(id: CategoryId.Of("67dc43ee9b19c6773e9cec41"), name: "MacBook Air", description: "MacBookAir category.", order: 0, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
        Category.Create(id: CategoryId.Of("68dc43ee9b19c6773e9cec42"), name: "MacBook Pro", description: "MacBook Pro category.", order: 1, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
        Category.Create(id: CategoryId.Of("69dc43ee9b19c6773e9cec43"), name: "iMac", description: "iMac category.", order: 2, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
        Category.Create(id: CategoryId.Of("70dc43ee9b19c6773e9cec44"), name: "Mac mini", description: "Mac mini category.", order: 3, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
        Category.Create(id: CategoryId.Of("71dc43ee9b19c6773e9cec45"), name: "Mac Studio", description: "Mac Studio category.", order: 4, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
        Category.Create(id: CategoryId.Of("72dc43ee9b19c6773e9cec46"), name: "Mac Pro", description: "Mac Pro category.", order: 5, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
        Category.Create(id: CategoryId.Of("73dc43ee9b19c6773e9cec47"), name: "Displays", description: "Displays category.", order: 6, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),

        Category.Create(id: CategoryId.Of("81c4708a9ee0a5e6fbafdabd"), name: "iPad Pro", description: "iPad Pro category.", order: 0, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
        Category.Create(id: CategoryId.Of("82dc4708a9ee0a5e6fbafdac"), name: "iPad Air", description: "iPad Air category.", order: 1, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
        Category.Create(id: CategoryId.Of("67dc5336a9ee0a5e6fbafdb3"), name: "iPad", description: "iPad category.", order: 2, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
        Category.Create(id: CategoryId.Of("67dc5338a9ee0a5e6fbafdb4"), name: "iPad mini", description: "iPad mini category.", order: 3, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
        Category.Create(id: CategoryId.Of("67dc533aa9ee0a5e6fbafdb5"), name: "Apple Pencil", description: "Apple Pencil category.", order: 4, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
        Category.Create(id: CategoryId.Of("67dc533ca9ee0a5e6fbafdb6"), name: "Keyboards", description: "Keyboards category.", order: 5, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),

        Category.Create(id: CategoryId.Of("91dc470aa9ee0a5e6fbafdbc"), name: "iPhone 16", description: "iPhone 16 category.", order: 0, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
        Category.Create(id: CategoryId.Of("92dc470aa9ee0a5e6fbafdbd"), name: "iPhone 15", description: "iPhone 15 category.", order: 1, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
        Category.Create(id: CategoryId.Of("93dc470aa9ee0a5e6fbafdbe"), name: "iPhone 14", description: "iPhone 14 category.", order: 2, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
        Category.Create(id: CategoryId.Of("94dc470aa9ee0a5e6fbafdbf"), name: "iPhone 13", description: "iPhone 13 category.", order: 3, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
        Category.Create(id: CategoryId.Of("95dc470aa9ee0a5e6fbafdbg"), name: "iPhone 12", description: "iPhone 12 category.", order: 4, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),

        Category.Create(id: CategoryId.Of("67dc5378a9ee0a5e6fbafdb8"), name: "Apple Watch Series 10", description: "Apple Watch Series 10 category.", order: 0, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),
        Category.Create(id: CategoryId.Of("67dc5379a9ee0a5e6fbafdb9"), name: "Apple Watch Series 9", description: "Apple Watch Series 9 category.", order: 1, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),
        Category.Create(id: CategoryId.Of("67dc537ba9ee0a5e6fbafdba"), name: "Apple Watch Ultra 2", description: "Apple Watch Ultra 2 category.", order: 2, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),
        Category.Create(id: CategoryId.Of("67dc537ca9ee0a5e6fbafdbb"), name: "Apple Watch SE 2", description: "Apple Watch SE 2 category.", order: 3, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),

        Category.Create(id: CategoryId.Of("67dc5397a9ee0a5e6fbafdbc"), name: "AirPods", description: "AirPods category.", order: 1, parentId: CategoryId.Of("67dc470ea9ee0a5e6fbafdad")),
        Category.Create(id: CategoryId.Of("67dc539aa9ee0a5e6fbafdbd"), name: "EarPods", description: "EarPods category.", order: 2, parentId: CategoryId.Of("67dc470ea9ee0a5e6fbafdad")),
    };
}
