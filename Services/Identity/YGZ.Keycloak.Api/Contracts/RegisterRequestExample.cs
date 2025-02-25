using NJsonSchema.Generation;
using NSwag;

namespace YGZ.Keycloak.Api.Contracts;

public class RegisterRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(RegisterRequest))
        {
            // Directly set the example as a raw JSON-like object
            context.Schema.Example = new 
            {
                email = "lov3rinve146@gmail.com",
                password = "password",
                first_name = "Bach",
                last_name = "Le"
            };
        }
    }
}
