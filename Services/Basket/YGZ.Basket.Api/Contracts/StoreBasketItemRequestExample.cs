using NJsonSchema.Generation;

namespace YGZ.Basket.Api.Contracts;

public class StoreBasketItemRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(StoreBasketItemRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                sku_id = "690f4601e2295b9f94f23f5f",
                quantity = 1
            };
        }
    }
}
