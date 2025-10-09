using NJsonSchema.Generation;

namespace YGZ.Basket.Api.Contracts;

public class StoreEventItemRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(StoreEventItemRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                event_item_id = "04edf970-b569-44ac-a116-9847731929ab"
            };
        }
    }
}