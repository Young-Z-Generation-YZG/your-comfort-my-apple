using NJsonSchema.Generation;

namespace YGZ.Identity.Api.Contracts.Tenants;

public class CreateTenantUserRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateTenantUserRequest))
        {
            context.Schema.Example = new
            {
                first_name = "Bach",
                last_name = "Le Xuan",
                email = "lov3rinve146",
                password = "password",
                phone_number = "0333284890"
            };
        }
    }
}