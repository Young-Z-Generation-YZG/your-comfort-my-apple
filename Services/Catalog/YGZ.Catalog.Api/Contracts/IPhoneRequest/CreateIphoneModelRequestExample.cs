using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.IphoneRequest;

public class CreateIphoneModelRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateIphoneModelRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                name = "iPhone 16",
                models = new List<object>()
                {
                    new
                    {
                        model_name = "IPHONE_16",
                        order = 0,
                    },
                    new
                    {
                        model_name = "IPHONE_16_PLUS",
                        order = 1,
                    },
                },
                colors = new List<object>()
                {
                    new
                    {
                        color_name = "ULTRAMARINE",
                        color_hex_code = "#3f51b5",
                        showcase_image_id = "test-id",
                        order = 0
                    }
                },
                storages = new List<object>()
                {
                    new
                    {
                        storage_name = "256GB",
                        storage_value =  256,
                        order = 0
                    }
                },
                description = "iPhone 16 ultramarine description.",
                showcase_images = new List<object>()
                {
                    new
                    {
                        image_id = "image_id_1",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0E5L1ZBdzY3RU1aTVdUR3lMZHFNVzE0RzhwM3RLeUk1S0YzTkJVVmF2Ly9R&traceId=1",
                        image_name = "iPhone 16",
                        image_description = "",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        order = 0,
                    }
                },
                category_id = "67346f7549189f7314e4ef0c"
            };
        }
    }
}
