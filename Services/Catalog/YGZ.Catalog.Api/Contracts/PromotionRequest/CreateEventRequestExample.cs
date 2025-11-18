using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public class CreateEventRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateEventRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                title = "New Year Sale",
                description = "Get 50% off on all items during the New Year Sale!",
                start_date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                end_date = new DateTime(2025, 1, 31, 23, 59, 59, DateTimeKind.Utc)
            };
        }
    }
}
