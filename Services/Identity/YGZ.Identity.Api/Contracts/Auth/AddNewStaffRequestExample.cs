using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Auth;

public class AddNewStaffRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(AddNewStaffRequest))
        {
            context.Schema.Example = new
            {
                email = "staff@example.com",
                password = "password123",
                first_name = "John",
                last_name = "Doe",
                phone_number = "0123456789",
                role_name = "STAFF",
                tenant_id = "tenant-123",
                branch_id = "branch-456"
            };
        }
    }
}
