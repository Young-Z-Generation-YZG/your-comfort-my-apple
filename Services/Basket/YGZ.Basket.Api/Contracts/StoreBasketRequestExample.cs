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
                        is_selected = false,
                        sku_id = "690f4601e2295b9f94f23f5f",
                        quantity = 1
                    }
                },
            };
        }
    }
}
