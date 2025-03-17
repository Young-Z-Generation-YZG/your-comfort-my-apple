using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts;


public class RegisterRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(RegisterRequest))
        {
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
