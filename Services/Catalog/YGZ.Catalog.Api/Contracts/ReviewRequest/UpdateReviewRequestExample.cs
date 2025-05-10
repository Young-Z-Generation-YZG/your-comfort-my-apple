using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.ReviewRequest;

public class UpdateReviewRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(UpdateReviewRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                content = "This is a review content.",
                rating = 5,
            };
        }
    }
}