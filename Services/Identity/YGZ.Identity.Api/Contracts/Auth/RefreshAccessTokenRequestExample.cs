using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Auth;

public class RefreshAccessTokenRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(RefreshAccessTokenRequest))
        {

        }
    }
}
