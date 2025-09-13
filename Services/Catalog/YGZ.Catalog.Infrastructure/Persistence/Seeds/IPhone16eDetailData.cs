
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public partial class Seeds
{
    public static IEnumerable<IPhone16Detail> IPhone16e_Details
    {
        get
        {
            var color_white = ColorBK.Create("white", "#FAFAFA", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-white-202409", null);
            var color_black = ColorBK.Create("black", "#3C4042", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202409", null);

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
}
