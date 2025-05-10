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
                order_id = "93ded754-2b7d-4a1f-9487-2af0f9fa6e09",
                order_item_id = "0101785f-bed7-4125-a869-099a01a95562",
                customer_username = "lov3rinve146@gmail.com",
                content = "This is a review content.",
                rating = 5,
            };
        }
    }
}