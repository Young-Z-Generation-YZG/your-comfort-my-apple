using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
                product_id = "67cbcff3cb422bbaf809c5a9",
                model_id = "67346f7549189f7314e4ef0c",
                content = "This is a review content.",
                rating = 5,
                order_item_id = "41922fd4-7050-4fa8-a2f2-1f580558ee88",
            };
        }
    }
}