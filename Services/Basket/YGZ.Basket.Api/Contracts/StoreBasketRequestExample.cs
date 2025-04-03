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
                        product_id = "67346f7549189f7314e4ef0c",
                        product_name = "iPhone 16 256GB",
                        product_color_name = "ultramarine",
                        product_unit_price = 799,
                        product_name_tag = "IPHONE",
                        product_image = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-7inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4OUp1NDJCalJ6MnpHSm1KdCtRZ0FvSDJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0R6SkNnaG1kYkl1VUVsNXVsVGJrQ0s0UmdXWi9jaTBCeEx5VFNDNXdWbmdB&traceId=1",
                        product_slug = "iphone-16-256gb",
                        category_id = "91dc470aa9ee0a5e6fbafdbc",
                        quantity = 2,
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
