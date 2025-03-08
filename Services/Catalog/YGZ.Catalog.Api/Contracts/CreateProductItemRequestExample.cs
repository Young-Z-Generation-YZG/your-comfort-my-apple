using Newtonsoft.Json.Schema;
using NJsonSchema.Generation;
using YGZ.Catalog.Api.Contracts.Common;
using YGZ.Catalog.Domain.Core.Enums;

namespace YGZ.Catalog.Api.Contracts;

// ...

public class CreateProductItemRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateProductItemRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                model = "iPhone 16",
                color = new
                {
                    color_name = "ultramarine",
                    color_hex = "#3f51b5",
                    color_order = 1
                },
                storage = 128,
                description = "iPhone 16 ultramarine description.",
                price = 799,
                images = new List<object>()
                {
                    new
                    {
                        image_id = "image_id_1",
                        image_url = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0E5L1ZBdzY3RU1aTVdUR3lMZHFNVzE0RzhwM3RLeUk1S0YzTkJVVmF2Ly9R&traceId=1",
                        image_order = 0
                    },
                    new
                    {
                        image_id = "image_id_2",
                        image_url = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV1?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJSc2VJb0FISFY1LzJCMlcyeCs0eEFUMngwVnJycmY0WkN2ZnNvOUpFNFY4aEdha0F6blFmTkM2c3RxTWJNZ2FFRnZ4QmRNWmQ0OCt3NmpBeisralBB&traceId=1",
                        image_order = 1
                    },
                    new
                    {
                        image_id = "image_id_3",
                        image_url = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV2?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzdvdDhQaEp3aGRlYWd5eFdZcE42WkFUMngwVnJycmY0WkN2ZnNvOUpFNFY4aEdha0F6blFmTkM2c3RxTWJNZ2FUdFJkVzN6TFNEeXlHU0FCcTd2ZUl3&traceId=1",
                        image_order = 2
                    },
                    new
                    {
                        image_id = "image_id_4",
                        image_url = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine_AV3?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzROSXA0bDVkMUh3bnUrQ3QzckNHbXdUMngwVnJycmY0WkN2ZnNvOUpFNFd0WXdwZkhSYStycUNlU1I0YzZvelp6Y1dsSjlld0l1dWVQb1Bmc3Q1MTd3&traceId=1",
                        image_order = 3
                    }
                },
                product_id = "67346f7549189f7314e4ef0c"
            };
        }
    }
}
