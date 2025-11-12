using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Auth;

public class RefreshTokenRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(RefreshTokenRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                refresh_token = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJ..."
            };
        }
    }
}
