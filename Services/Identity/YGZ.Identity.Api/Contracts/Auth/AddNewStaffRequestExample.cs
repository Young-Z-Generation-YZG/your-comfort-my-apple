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
                birth_day = "1990-01-01",
                first_name = "John",
                last_name = "Doe",
                email = "john.doe@example.com",
                password = "SecurePassword123",
                phone_number = "0987654321",
                role_name = "STAFF",
                tenant_id = "664355f845e56534956be32b",
                branch_id = "664357a235e84033bbd0e6b6"
            };
        }
    }
}
