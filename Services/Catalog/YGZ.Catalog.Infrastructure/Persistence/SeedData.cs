using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence;

public class SeedData
{
    public static IEnumerable<IPhone16Model> IPhone16Models => new List<IPhone16Model>
    {
        new IPhone16Model(IPhone16ModelId.Of("67346f7549189f7314e4ef0c"))
        {
            Models = new List<Model>
            {
                Model.Create(modelName: "iPhone 16",
                             modelOrder: 0),
                Model.Create(modelName: "iPhone 16 PLus",
                             modelOrder: 1),
            },
            Colors = new List<Color>
            {
                Color.Create(colorName: "ultramarine",
                             colorHex: "#3f51b5",
                             colorOrder: 0)
            },
            Storages = new List<Storage>
            {
                Storage.STORAGE_128,
                Storage.STORAGE_256,
                Storage.STORAGE_512,
                Storage.STORAGE_1024,
            },
            Description = "",
            OverallSold = 0,
            AverageRating = AverageRating.Create(0, 0),
            RatingStars = new List<RatingStar>
            {
                RatingStar.Create(1, 0),
                RatingStar.Create(2, 0),
                RatingStar.Create(3, 0),
                RatingStar.Create(4, 0),
                RatingStar.Create(5, 0),
            },
            DescriptionImages = new List<Image>
            {
                Image.Create(imageId: "image_id_1",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0E5L1ZBdzY3RU1aTVdUR3lMZHFNVzE0RzhwM3RLeUk1S0YzTkJVVmF2Ly9R&traceId=1",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
            }
        }
    };

    public static IEnumerable<IPhone16Detail> IPhone16Details => new List<IPhone16Detail>
    {
        new IPhone16Detail(IPhone16Id.Of("67cbcff3cb422bbaf809c5a9"))
        {
            Model = "iPhone 16",
            Color = Color.Create("ultramarine", "#3f51b5", null),
            Storage = Storage.STORAGE_256,
            UnitPrice = 799,
            Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
            AvailableInStock = 100,
            TotalSold = 0,
            State = State.ACTIVE,
            Images = new Image[]
            {
                Image.Create(imageId: "image_id_1",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0E5L1ZBdzY3RU1aTVdUR3lMZHFNVzE0RzhwM3RLeUk1S0YzTkJVVmF2Ly9R&traceId=1",
                             imageName: "iPhone 16",
                             imageDescription: "Place first",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 0),
                Image.Create(imageId: "image_id_2",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV1?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJSc2VJb0FISFY1LzJCMlcyeCs0eEFUMngwVnJycmY0WkN2ZnNvOUpFNFY4aEdha0F6blFmTkM2c3RxTWJNZ2FFRnZ4QmRNWmQ0OCt3NmpBeisralBB&traceId=1",
                             imageName: "iPhone 16",
                             imageDescription: "Place second",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 1),
                Image.Create(imageId: "image_id_3",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV2?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzdvdDhQaEp3aGRlYWd5eFdZcE42WkFUMngwVnJycmY0WkN2ZnNvOUpFNFY4aEdha0F6blFmTkM2c3RxTWJNZ2FUdFJkVzN6TFNEeXlHU0FCcTd2ZUl3&traceId=1",
                             imageName: "iPhone 16",
                             imageDescription: "Place third",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 2),
                Image.Create(imageId: "image_id_4",
                             imageUrl: "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV3?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzROSXA0bDVkMUh3bnUrQ3QzckNHbXdUMngwVnJycmY0WkN2ZnNvOUpFNFd0WXdwZkhSYStycUNlU1I0YzZvelp6Y1dsSjlld0l1dWVQb1Bmc3Q1MTd3&traceId=1",
                             imageName: "iPhone 16",
                             imageDescription: "Place four",
                             imageWidth: 0,
                             imageHeight: 0,
                             imageBytes: 0,
                             imageOrder: 3),
            },
            Slug = Slug.Create("iphone 16"),
            IPhoneModelId = IPhone16ModelId.Of("67346f7549189f7314e4ef0c")
        }
    };


}
