using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.SkuRequestRequest;

public class CreateSkuRequestRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateSkuRequestRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                sender_user_id = "65dad719-7368-4d9f-b623-f308299e9575",
                from_branch_id = "664357a235e84033bbd0e6b6",
                to_branch_id = "690e034dff79797b05b3bc88",
                sku_id = "690f4601e2295b9f94f23f5f",
                request_quantity = 10,
            };
        }
    }
}
