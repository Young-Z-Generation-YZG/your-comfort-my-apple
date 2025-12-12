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
                roles = new List<string> { "ADMIN", "STAFF" }
            };
        }
    }
}
