using NJsonSchema.Generation;
using YGZ.BuildingBlocks.Shared.Enums;

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
                name = "1060 KVC TD",
                sub_domain = "hcm-td-kvc-1060",
                branch_address = "Số 1060, Kha Vạn Cân, Linh Chiểu, Thủ Đức",
                tenant_type = ETenantType.BRANCH.Name,
                tenant_description = "tenant_description",
                branch_description = "branch_description",
            };
        }
    }
}