using NJsonSchema.Generation;

namespace YGZ.Ordering.Api.Contracts;

public class CreateOrderRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateOrderRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                orders = new List<object>()
                {
                    new
                    {
                        product_id = "67346f7549189f7314e4ef0c",
                        product_model = "iPhone 16",
                        product_color = "ultramarine",
                        product_color_hex = "#3f51b5",
                        product_storage = 128,
                        product_unit_price = 799,
                        product_image = "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-7inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4OUp1NDJCalJ6MnpHSm1KdCtRZ0FvSDJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0R6SkNnaG1kYkl1VUVsNXVsVGJrQ0s0UmdXWi9jaTBCeEx5VFNDNXdWbmdB&traceId=1",
                        quantity = 1
                    }
                },
                shipping_address = new
                {
                    contact_name = "Bach Le",
                    contact_email = "lov3rinve146@gmail.com",
                    contact_phone_number = "0333284890",
                    address_line = "106* Kha Van Can",
                    district = "Thu Duc",
                    province = "Ho Chi Minh",
                    country = "Việt Nam",
                },
                payment_method = "VNPAY",
                discount_amount = 0,
                sub_total = 799,
                total = 799
            };
        }
    }
}
