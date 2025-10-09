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
                        model_id = "68e403d5617b27ad030bf28f",
                        model = new
                        {
                            name = "iPhone 15",
                            normalized_name = "IPHONE_15"
                        },
                        color = new
                        {
                            name = "Blue",
                            normalized_name = "BLUE"
                        },
                        storage = new
                        {
                            name = "128GB",
                            normalized_name = "128GB"
                        },
                        quantity = 1
                    }
                },
            };
        }
    }
}
