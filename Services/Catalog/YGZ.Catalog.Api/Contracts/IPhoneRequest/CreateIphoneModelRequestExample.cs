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
                name = "iPhone 15",
                models = new List<object>()
                {
                    new
                    {
                        model_name = "iPhone 15",
                        order = 0,
                    },
                    new
                    {
                        model_name = "iPhone 15 Plus",
                        order = 1,
                    },
                },
                colors = new List<object>()
                {
                    new
                    {
                        color_name = "Blue",
                        color_hex_code = "#D5DDDF",
                        showcase_image_id = "iphone-15-finish-select-202309-6-1inch-blue_zgxzmz",
                        order = 0
                    },
                    new
                    {
                        color_name = "Pink",
                        color_hex_code = "#EBD3D4",
                        showcase_image_id = "iphone-15-finish-select-202309-6-1inch-pink_j6v96t",
                        order = 1
                    },
                    new
                    {
                        color_name = "Yellow",
                        color_hex_code = "#EDE6C8",
                        showcase_image_id = "iphone-15-finish-select-202309-6-1inch-yellow_pwviwe",
                        order = 2
                    },
                    new
                    {
                        color_name = "Green",
                        color_hex_code = "#D0D9CA",
                        showcase_image_id = "iphone-15-finish-select-202309-6-1inch-green_yk0ln5",
                        order = 3
                    },
                    new
                    {
                        color_name = "Black",
                        color_hex_code = "#4B4F50",
                        showcase_image_id = "iphone-15-finish-select-202309-6-1inch-black_hhhvfs",
                        order = 4
                    },
                },
                storages = new List<object>()
                {
                    new
                    {
                        storage_name = "128GB",
                        storage_value =  128,
                        order = 0
                    },
                    new
                    {
                        storage_name = "256GB",
                        storage_value =  256,
                        order = 1
                    },
new
                    {
                        storage_name = "512GB",
                        storage_value =  512,
                        order = 2
                    },
                    new
                    {
                        storage_name = "1TB",
                        storage_value =  1024,
                        order = 3
                    }
                },
                description = "iPhone 15 model description.",
                showcase_images = new List<object>()
                {
                    new
                    {
                        image_id = "iphone-15-finish-select-202309-6-1inch-blue_zgxzmz",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp",
                        image_name = "iPhone 15 blue",
                        image_description = "",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        order = 0,
                    },
                    new
                    {
                        image_id = "iphone-15-finish-select-202309-6-1inch-pink_j6v96t",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp",
                        image_name = "iPhone 15 pink",
                        image_description = "",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        order = 1,
                    },
                    new
                    {
                        image_id = "iphone-15-finish-select-202309-6-1inch-yellow_pwviwe",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp",
                        image_name = "iPhone 15 yellow",
                        image_description = "",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        order = 2,
                    },
                    new
                    {
                        image_id = "iphone-15-finish-select-202309-6-1inch-green_yk0ln5",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp",
                        image_name = "iPhone 15 green",
                        image_description = "",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        order = 3,
                    },
                    new
                    {
                        image_id = "iphone-15-finish-select-202309-6-1inch-black_hhhvfs",
                        image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp",
                        image_name = "iPhone 15 black",
                        image_description = "",
                        image_width = 0,
                        image_height = 0,
                        image_bytes = 0,
                        order = 4,
                    },
                },
                category_id = "67346f7549189f7314e4ef0c"
            };
        }
    }
}
