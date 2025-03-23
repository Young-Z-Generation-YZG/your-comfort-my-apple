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
                Cart_items = new List<object>()
                {
                    new
                    {
                        product_id = "67346f7549189f7314e4ef0c",
                        product_model = "iPhone 16",
                        product_color = "ultramarine",
                        product_color_hex = "#3f51b5",
                        product_storage = 128,
                        product_unit_price = 799,
                        product_name_tag = "IPHONE",
                        product_image = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-7inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4OUp1NDJCalJ6MnpHSm1KdCtRZ0FvSDJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0R6SkNnaG1kYkl1VUVsNXVsVGJrQ0s0UmdXWi9jaTBCeEx5VFNDNXdWbmdB&traceId=1",
                        quantity = 1
                    }
                }
            };
        }
    }
}
