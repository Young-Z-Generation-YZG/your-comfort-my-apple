using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;


public class CreatePromotionEventRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreatePromotionEventRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                event_title = "New Year Sale",
                event_description = "Get 50% off on all items during the New Year Sale!",
                event_state = "active",
                event_valid_from = new DateTime(2025, 4, 30).ToLocalTime(), // Unix timestamp for 2023-01-01 00:00:00
                event_valid_to = new DateTime(2025, 4, 30).ToUniversalTime(), // ISO 8601 format
            };
        }
    }
}