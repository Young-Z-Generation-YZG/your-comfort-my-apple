using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Auth;

public class LoginRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(LoginRequest))
        {
            context.Schema.Example = new
            {
                email = "lov3rinve146@gmail.com",
                password = "password",
            };
        }
    }
}