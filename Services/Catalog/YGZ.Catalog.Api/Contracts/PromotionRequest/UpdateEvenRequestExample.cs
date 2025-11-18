using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public class UpdateEvenRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(UpdateEvenRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                title = "Sale 12/12",
                description = "Sale items in shop with special price",
                start_date = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                end_date = new DateTime(2025, 12, 29, 23, 59, 59, DateTimeKind.Utc),
                add_event_items = new[]
                {
                    new
                    {
                        sku_id = "690f4601e2295b9f94f23f5f",
                        discount_type = "PERCENTAGE",
                        discount_value = 10,
                        stock = 5
                    }
                },
                remove_event_item_ids = new[] { "event_item_id_1" }
            };
        }
    }
}
