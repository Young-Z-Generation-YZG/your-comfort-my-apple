using NJsonSchema.Generation;

namespace YGZ.Basket.Api.Contracts;

public class CheckoutBasketRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CheckoutBasketRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                shipping_address = new
                {
                    contact_name = "Foo Bar",
                    contact_phone_number = "0333284890",
                    address_line = "123 Street",
                    district = "Thu Duc",
                    province = "Ho Chi Minh",
                    country = "Việt Nam"
                },
                payment_method = "VNPAY",
                discount_code = "APRIL2025",
            };
        }
    }
}
