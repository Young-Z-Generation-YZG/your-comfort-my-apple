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
                        model_id = "664351e90087aa09993f5ae7",
                        sku_id = "690f4601e2295b9f94f23f5f",
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
