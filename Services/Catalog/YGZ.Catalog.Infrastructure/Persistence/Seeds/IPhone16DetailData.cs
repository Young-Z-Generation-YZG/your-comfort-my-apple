

using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public partial class Seeds
{
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
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811364/iphone-16-finish-select-202409-6-1inch-ultramarine_AV1_qbjznq.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811375/iphone-16-finish-select-202409-6-1inch-ultramarine_AV2_apdetn.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811387/iphone-16-finish-select-202409-6-1inch-ultramarine_AV3_s7rdc9.webp",
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
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811404/iphone-16-finish-select-202409-6-1inch-teal_gfumfa.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811411/iphone-16-finish-select-202409-6-1inch-teal_AV1_xkxfxu.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811425/iphone-16-finish-select-202409-6-1inch-teal_AV2_bszuzd.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811431/iphone-16-finish-select-202409-6-1inch-teal_AV3_ngsokk.webp",
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
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811098/iphone-16-finish-select-202409-6-1inch-pink_q2saue.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811146/iphone-16-finish-select-202409-6-1inch-pink_AV1_ri56uu.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811161/iphone-16-finish-select-202409-6-1inch-pink_AV2_hyjn9b.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811170/iphone-16-finish-select-202409-6-1inch-pink_AV3_ihm1mu.webp",
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
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811448/iphone-16-finish-select-202409-6-1inch-white_ghumel.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811462/iphone-16-finish-select-202409-6-1inch-white_AV1_qnax3q.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811468/iphone-16-finish-select-202409-6-1inch-white_AV2_lo7nxq.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811481/iphone-16-finish-select-202409-6-1inch-white_AV3_mpbrpa.webp",
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
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811496/iphone-16-finish-select-202409-6-1inch-black_wnfzl5.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811502/iphone-16-finish-select-202409-6-1inch-black_AV1_azosbf.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811527/iphone-16-finish-select-202409-6-1inch-black_AV2_ytj4vb.webp",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811534/iphone-16-finish-select-202409-6-1inch-black_AV3_zcyu1p.webp",
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
}
