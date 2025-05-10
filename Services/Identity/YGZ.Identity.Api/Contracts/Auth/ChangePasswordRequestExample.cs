
using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Auth;

public class ChangePasswordRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(ChangePasswordRequest))
        {
            context.Schema.Example = new
            {
                old_password = "oldpassword",
                new_password = "newpassword",
            };
        }
    }
}