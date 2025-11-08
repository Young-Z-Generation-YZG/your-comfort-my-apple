
using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.ReviewRequest;

public class CreateReviewRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateReviewRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                sku_id = "67cbcff3cb422bbaf809c5a9",
                order_id = "93ded754-2b7d-4a1f-9487-2af0f9fa6e09",
                order_item_id = "0101785f-bed7-4125-a869-099a01a95562",
                content = "This is a review content.",
                rating = 5,
            };
        }
    }
}