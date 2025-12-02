using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Auth;

public class AssignRolesRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(AssignRolesRequest))
        {
            context.Schema.Example = new
            {
                user_id = "be0cd669-237a-484d-b3cf-793e0ad1b0ea",
                roles = new List<string> { "ADMIN", "STAFF" }
            };
        }
    }
}
