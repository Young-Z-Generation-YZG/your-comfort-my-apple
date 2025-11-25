using NJsonSchema.Generation;

namespace YGZ.Basket.Api.Contracts
{
    public class CheckoutBasketWithBlockchainRequestExample : ISchemaProcessor
    {
        public void Process(SchemaProcessorContext context)
        {
            if (context.ContextualType.Type == typeof(CheckoutBasketWithBlockchainRequest))
            {
                var schema = context.Schema;

                schema.Example = new
                {
                    crypto_uuid = "550e8400-e29b-41d4-a716-446655440000",
                    shipping_address = new
                    {
                        contact_name = "Foo Bar",
                        contact_phone_number = "0333284890",
                        address_line = "123 Street",
                        district = "Thu Duc",
                        province = "Ho Chi Minh",
                        country = "Việt Nam"
                    },
                    payment_method = "SOLANA",
                    discount_code = "APRIL2025",
                    customer_public_key = "7xKXtg2CW87d97TXJSDpbD5jBkheTqA83TZRuJosgAsU",
                    tx = "5VERv8NMvzbJMEkV8xnrLkEaWRtSz9CosKDYjCJjBRnbJLgp8uirBgmQpjKhoR4tjF3ZpRzrFmBV6UjKdiSZkQUW"
                };
            }
        }
    }
}
