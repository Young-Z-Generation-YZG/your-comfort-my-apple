using NJsonSchema.Generation;

namespace YGZ.Basket.Api.Contracts;

public class StoreBasketRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(StoreBasketRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                cart_items = new List<object>()
                {
                    new
                    {
                        product_id = "67cbcff3cb422bbaf809c5a9",
                        model_id = "67346f7549189f7314e4ef0c",
                        product_name = "iPhone 16 128GB",
                        product_color_name = "ultramarine",
                        product_unit_price = 699,
                        product_name_tag = "IPHONE",
                        product_image = "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp",
                        product_slug = "iphone-16-128gb",
                        category_id = "91dc470aa9ee0a5e6fbafdbc",
                        quantity = 1,
                        promotion = new
                        {
                            promotion_id_or_code = "f55f322f-6406-4dfa-b2ea-2777f7813e70",
                            promotion_event_type = "PROMOTION_EVENT",
                        },
                        order_index = 1,
                    }
                },
            };
        }
    }
}
