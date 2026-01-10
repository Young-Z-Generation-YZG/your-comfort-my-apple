using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.SkuRequestRequest;

public class UpdateSkuRequestRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(UpdateSkuRequestRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                state = "APPROVED",
            };
        }
    }
}
