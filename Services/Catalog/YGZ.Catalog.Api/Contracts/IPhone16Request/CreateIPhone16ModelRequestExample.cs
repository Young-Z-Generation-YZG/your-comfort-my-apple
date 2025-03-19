using NJsonSchema.Generation;
using YGZ.Catalog.Api.Contracts.IPhone16;

namespace YGZ.Catalog.Api.Contracts.IPhone16Request;

public class CreateIPhone16ModelRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateIPhone16ModelRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                models = new List<object>()
                {
                    new
                    {
                        model_name = "iPhone 16",
                        model_order = 0,
                    },
                    new
                    {
                        model_name = "iPhone 16 plus",
                        model_order = 1,
                    },
                },
                colors = new List<object>()
                {
                    new
                    {
                        color_name = "ultramarine",
                        color_hex = "#3f51b5",
                        color_order = 0
                    }
                },
                storages = new List<int>()
                {
                    128,
                    256,
                    512,
                    1024
                },
                description = "iPhone 16 ultramarine description.",
                description_images = new List<object>()
                {
                    new
                    {
                        image_id = "image_id_1",
                        image_url = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0E5L1ZBdzY3RU1aTVdUR3lMZHFNVzE0RzhwM3RLeUk1S0YzTkJVVmF2Ly9R&traceId=1",
                        image_name = "iPhone 16",
                        image_description = "display first",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        image_order = 0,
                    }
                },
                category_id = "67346f7549189f7314e4ef0c"
            };
        }
    }
}
