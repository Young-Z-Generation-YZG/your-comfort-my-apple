

using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Extensions;

public class SeedData
{
    public static IEnumerable<ProductItem> ProductItems => new List<ProductItem>
    {
        new ProductItem(ProductItemId.ToValueObjectId("67cbcff3cb422bbaf809c5a9"))
        {
            Model = "iPhone 16",
            Color = Color.Create("ultramarine", "#3f51b5", 1),
            Storage = StorageEnum.STORAGE_256,
            Price = 799,
            Description = "The iPhone 16 is a smartphone designed, developed, and marketed by Apple Inc. It is the fourteenth generation of the iPhone, alongside the iPhone 16 Pro and iPhone 16 Pro Max models, and was announced on September 14, 2021. Pre-orders began on October 15, 2021, and the phone was officially released on October 22, 2021.",
            AvailableInStock = 100,
            Sold = 0,
            State = StateEnum.ACTIVE,
            Images = new Image[]
            {
                Image.Create("image_id_1",
                             "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0E5L1ZBdzY3RU1aTVdUR3lMZHFNVzE0RzhwM3RLeUk1S0YzTkJVVmF2Ly9R&traceId=1",
                             0),
                Image.Create("image_id_2",
                             "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV1?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJSc2VJb0FISFY1LzJCMlcyeCs0eEFUMngwVnJycmY0WkN2ZnNvOUpFNFY4aEdha0F6blFmTkM2c3RxTWJNZ2FFRnZ4QmRNWmQ0OCt3NmpBeisralBB&traceId=1",
                             1),
                Image.Create("image_id_3",
                             "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV2?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzdvdDhQaEp3aGRlYWd5eFdZcE42WkFUMngwVnJycmY0WkN2ZnNvOUpFNFY4aEdha0F6blFmTkM2c3RxTWJNZ2FUdFJkVzN6TFNEeXlHU0FCcTd2ZUl3&traceId=1",
                             2),
                Image.Create("image_id_4",
                             "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV3?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzROSXA0bDVkMUh3bnUrQ3QzckNHbXdUMngwVnJycmY0WkN2ZnNvOUpFNFd0WXdwZkhSYStycUNlU1I0YzZvelp6Y1dsSjlld0l1dWVQb1Bmc3Q1MTd3&traceId=1",
                             3)
            },
            ProductId = ProductId.ToValueObjectId("67346f7549189f7314e4ef0c")
        }
    };
}
