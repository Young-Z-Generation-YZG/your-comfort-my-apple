using NJsonSchema.Generation;
using YGZ.Catalog.Api.Contracts.IPhone16;

namespace YGZ.Catalog.Api.Contracts.IPhone16Request;

public class CreateIPhone16DetailRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateIPhone16DetailRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                model = "iPhone 16",
                color = new
                {
                    color_name = "ultramarine",
                    color_hex = "#3f51b5",
                    color_image = "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-ultramarine-202409",
                },
                storage = 128,
                description = "iPhone 16 ultramarine description.",
                unit_price = 799,
                images = new List<object>()
                {
                    new
                    {
                        image_id = "image_id_1",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp",
                        image_name = "iPhone 16",
                        image_description = "display first",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        image_order = 0
                    },
                    new
                    {
                        image_id = "image_id_2",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744811364/iphone-16-finish-select-202409-6-1inch-ultramarine_AV1_qbjznq.webp",
                        image_name = "iPhone 16",
                        image_description = "display second",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        image_order = 1
                    },
                    new
                    {
                        image_id = "image_id_3",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744811375/iphone-16-finish-select-202409-6-1inch-ultramarine_AV2_apdetn.webp",
                        image_name = "iPhone 16",
                        image_description = "display third",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        image_order = 2
                    },
                    new
                    {
                        image_id = "image_id_4",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744811387/iphone-16-finish-select-202409-6-1inch-ultramarine_AV3_s7rdc9.webp",
                        image_name = "iPhone 16",
                        image_description = "display four",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        image_order = 3
                    }
                },
                iphone_model_id = "67346f7549189f7314e4ef0c"
            };
        }
    }
}
