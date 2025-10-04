using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.TenantRequest;

public class CreateTenantRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateTenantRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                name = "YB Store Truong Chinh",
                branch_address = "so 1023 Truong Chinh, Thanh Xuan, Ha Noi",
                tenant_description = "tenant_description",
                branch_description = "branch_description",
            };
        }
    }
}