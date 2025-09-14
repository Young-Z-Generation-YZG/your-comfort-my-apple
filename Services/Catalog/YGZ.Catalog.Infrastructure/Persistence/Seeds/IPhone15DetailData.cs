namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public partial class Seeds
{
    //public static IEnumerable<IPhone16Detail> IPhone15_Details
    //{
    //    get
    //    {
    //        var color_blue = ColorBK.Create(colorName: "blue", colorHex: "#D5DDDF", colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-blue-202309", colorOrder: 0);
    //        var color_pink = ColorBK.Create(colorName: "pink", colorHex: "#EBD3D4", colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-pink-202309", colorOrder: 1);
    //        var color_yellow = ColorBK.Create(colorName: "yellow", colorHex: "#EDE6C8", colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-yellow-202309", colorOrder: 2);
    //        var color_green = ColorBK.Create(colorName: "green", colorHex: "#D0D9CA", colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-green-202309", colorOrder: 3);
    //        var color_black = ColorBK.Create(colorName: "black", colorHex: "#4B4F50", colorImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202309", colorOrder: 4);

    //        var imageList_blue = new List<Image>
    //        {
    //            Image.Create(imageId: "image_id_1",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place first",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 0),
    //            Image.Create(imageId: "image_id_2",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960342/iphone-15-finish-select-202309-6-1inch-blue_AV1_GEO_US_kl7pt0.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place second",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 1),
    //            Image.Create(imageId: "image_id_3",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960347/iphone-15-finish-select-202309-6-1inch-blue_AV2_uhs3la.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place third",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 2),
    //        };

    //        var imageList_pink = new List<Image>
    //        {
    //            Image.Create(imageId: "image_id_1",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place first",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 0),
    //            Image.Create(imageId: "image_id_2",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960366/iphone-15-finish-select-202309-6-1inch-pink_AV1_GEO_US_e4b0sh.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place second",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 1),
    //            Image.Create(imageId: "image_id_3",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960380/iphone-15-finish-select-202309-6-1inch-pink_AV2_pdwa9k.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place third",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 2),
    //        };

    //        var imageList_yellow = new List<Image>
    //        {
    //            Image.Create(imageId: "image_id_1",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place first",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 0),
    //            Image.Create(imageId: "image_id_2",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960399/iphone-15-finish-select-202309-6-1inch-yellow_AV1_GEO_US_islgez.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place second",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 1),
    //            Image.Create(imageId: "image_id_3",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960407/iphone-15-finish-select-202309-6-1inch-yellow_AV2_vfyu3b.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place third",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 2),
    //        };

    //        var imageList_green = new List<Image>
    //        {
    //            Image.Create(imageId: "image_id_1",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place first",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 0),
    //            Image.Create(imageId: "image_id_2",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960454/iphone-15-finish-select-202309-6-1inch-green_AV1_GEO_US_rfogg6.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place second",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 1),
    //            Image.Create(imageId: "image_id_3",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960462/iphone-15-finish-select-202309-6-1inch-green_AV2_gnelrm.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place third",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 2),
    //        };

    //        var imageList_black = new List<Image>
    //        {
    //            Image.Create(imageId: "image_id_1",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place first",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 0),
    //            Image.Create(imageId: "image_id_2",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960477/iphone-15-finish-select-202309-6-1inch-black_AV1_GEO_US_g8nutl.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place second",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 1),
    //            Image.Create(imageId: "image_id_3",
    //                         imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960481/iphone-15-finish-select-202309-6-1inch-black_AV2_eferyd.webp",
    //                         imageName: "iPhone 15",
    //                         imageDescription: "Place third",
    //                         imageWidth: 0,
    //                         imageHeight: 0,
    //                         imageBytes: 0,
    //                         imageOrder: 2),
    //        };

    //        // BLUE
    //        var ip_15_blue_128 = new IPhone16Detail(IPhone16Id.Of("664351ae0087aa09993f5dc5"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_blue_256 = new IPhone16Detail(IPhone16Id.Of("664351c80087aa09993f5dcb"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_blue_512 = new IPhone16Detail(IPhone16Id.Of("664351e90087aa09993f5dd7"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_blue_1024 = new IPhone16Detail(IPhone16Id.Of("664355f845e56534956beaab"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // PINK
    //        var ip_15_pink_128 = new IPhone16Detail(IPhone16Id.Of("664351ae0087aa09993f5ab5"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_pink_256 = new IPhone16Detail(IPhone16Id.Of("664351c80087aa09993f5acb"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_pink_512 = new IPhone16Detail(IPhone16Id.Of("664351e90087aa09993f5ae7"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_pink_1024 = new IPhone16Detail(IPhone16Id.Of("664355f845e56534956be32b"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // YELLOW
    //        var ip_15_yellow_128 = new IPhone16Detail(IPhone16Id.Of("664357a235e84033bbd0e6b6"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_yellow_256 = new IPhone16Detail(IPhone16Id.Of("664357c335e84033bbd0e6bc"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_yellow_512 = new IPhone16Detail(IPhone16Id.Of("664357d735e84033bbd0e6c2"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_yellow_1024 = new IPhone16Detail(IPhone16Id.Of("6643581f35e84033bbd0e6c8"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // GREEN
    //        var ip_15_green_128 = new IPhone16Detail(IPhone16Id.Of("6643585335e84033bbd0e6d4"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_green_256 = new IPhone16Detail(IPhone16Id.Of("6643587435e84033bbd0e6da"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_green_512 = new IPhone16Detail(IPhone16Id.Of("6643590035e84033bbd0e6e7"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_green_1024 = new IPhone16Detail(IPhone16Id.Of("6643598a35e84033bbd0e6ed"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // BLACK
    //        var ip_15_black_128 = new IPhone16Detail(IPhone16Id.Of("664359cb35e84033bbd0e6f3"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_black_256 = new IPhone16Detail(IPhone16Id.Of("664359e535e84033bbd0e6f9"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_black_512 = new IPhone16Detail(IPhone16Id.Of("6645fb38004b89d6014f32ca"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_black_1024 = new IPhone16Detail(IPhone16Id.Of("664665f94021579ca709eed6"))
    //        {
    //            Model = "iPhone 15",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // 15 PLUS BLUE
    //        var ip_15_plus_blue_128 = new IPhone16Detail(IPhone16Id.Of("664067331c54661949f71eee"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_blue_256 = new IPhone16Detail(IPhone16Id.Of("66434f82539238006edd731d"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_blue_512 = new IPhone16Detail(IPhone16Id.Of("664439317954a1ae3c52364e"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_blue_1024 = new IPhone16Detail(IPhone16Id.Of("6645cf80af463a6f3221b481"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_blue,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_blue,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // 15 PLUS PINK
    //        var ip_15_plus_pink_128 = new IPhone16Detail(IPhone16Id.Of("6646d53550daf961d0f72f3c"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_pink_256 = new IPhone16Detail(IPhone16Id.Of("664b996dd679b4429afed9db"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_pink_512 = new IPhone16Detail(IPhone16Id.Of("673fdbc52a649976e0f9ad2f"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_pink_1024 = new IPhone16Detail(IPhone16Id.Of("673837eecaae94494c4ae174"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_pink,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_pink,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // 15 PLUS YELLOW
    //        var ip_15_plus_yellow_128 = new IPhone16Detail(IPhone16Id.Of("673e1d41caae94494c4af3b2"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_yellow_256 = new IPhone16Detail(IPhone16Id.Of("673ef9ffc9e9b8e0fd5b07f6"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_yellow_512 = new IPhone16Detail(IPhone16Id.Of("673efa00c9e9b8e0fd5b07fd"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_yellow_1024 = new IPhone16Detail(IPhone16Id.Of("673efa02c9e9b8e0fd5b0804"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_yellow,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_yellow,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // 15 PLUS GREEN
    //        var ip_15_plus_green_128 = new IPhone16Detail(IPhone16Id.Of("673efbddc9e9b8e0fd5b0908"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_green_256 = new IPhone16Detail(IPhone16Id.Of("673fffa64731feb253f19672"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_green_512 = new IPhone16Detail(IPhone16Id.Of("67403ea16a01c37300dfac28"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_green_1024 = new IPhone16Detail(IPhone16Id.Of("674954ef014f100d0c876b73"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_green,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_green,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };

    //        // 15 PLUS BLACK
    //        var ip_15_plus_black_128 = new IPhone16Detail(IPhone16Id.Of("674954f1014f100a0c876b7c"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_128,
    //            UnitPrice = 799,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_128.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_black_256 = new IPhone16Detail(IPhone16Id.Of("674cae82014f100b0c876daa"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_256,
    //            UnitPrice = 899,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_256.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_black_512 = new IPhone16Detail(IPhone16Id.Of("66434c34fd0d76dfa0abc6d8"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_512,
    //            UnitPrice = 1099,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_512.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };
    //        var ip_15_plus_black_1024 = new IPhone16Detail(IPhone16Id.Of("66434d1afd0d76dfa0def8af"))
    //        {
    //            Model = "iPhone 15 Plus",
    //            Color = color_black,
    //            Storage = Storage.STORAGE_1024,
    //            UnitPrice = 1299,
    //            GeneralModel = "iphone-15",
    //            Description = "The iPhone 15 Plus is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
    //            AvailableInStock = 100,
    //            TotalSold = 0,
    //            State = State.ACTIVE,
    //            Images = imageList_black,
    //            Slug = Slug.Create($"iphone 15 plus {Storage.STORAGE_1024.Name}"),
    //            IPhoneModelId = IPhone16ModelId.Of("67c1871284c95016a7573f10"),
    //            CategoryId = CategoryId.Of("92dc470aa9ee0a5e6fbafdbd")
    //        };


    //        return new List<IPhone16Detail>
    //        {
    //            ip_15_blue_128,
    //            ip_15_blue_256,
    //            ip_15_blue_512,
    //            ip_15_blue_1024,
    //            ip_15_pink_128,
    //            ip_15_pink_256,
    //            ip_15_pink_512,
    //            ip_15_pink_1024,
    //            ip_15_yellow_128,
    //            ip_15_yellow_256,
    //            ip_15_yellow_512,
    //            ip_15_yellow_1024,
    //            ip_15_green_128,
    //            ip_15_green_256,
    //            ip_15_green_512,
    //            ip_15_green_1024,
    //            ip_15_black_128,
    //            ip_15_black_256,
    //            ip_15_black_512,
    //            ip_15_black_1024,
    //            ip_15_plus_blue_128,
    //            ip_15_plus_blue_256,
    //            ip_15_plus_blue_512,
    //            ip_15_plus_blue_1024,
    //            ip_15_plus_pink_128,
    //            ip_15_plus_pink_256,
    //            ip_15_plus_pink_512,
    //            ip_15_plus_pink_1024,
    //            ip_15_plus_yellow_128,
    //            ip_15_plus_yellow_256,
    //            ip_15_plus_yellow_512,
    //            ip_15_plus_yellow_1024,
    //            ip_15_plus_green_128,
    //            ip_15_plus_green_256,
    //            ip_15_plus_green_512,
    //            ip_15_plus_green_1024,
    //            ip_15_plus_black_128,
    //            ip_15_plus_black_256,
    //            ip_15_plus_black_512,
    //            ip_15_plus_black_1024
    //        };
    //    }
    //}
}
