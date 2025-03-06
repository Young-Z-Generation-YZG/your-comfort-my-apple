using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts;

public class CreateProductItemRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateProductItemRequest))
        {
            context.Schema.Example = new 
            {
                model = "iPhone 16",
                color = "ultramarine",
                storage = 128,
                description = "iPhone 16 ultramarine description.",
                price = 799,
                quantity_in_stock = 100,
                product_id = "67346f7549189f7314e4ef0c"
            };
        }
    }
}