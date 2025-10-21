using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Tenant
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Tenant.DoesNotExist", message: "Tenant does not exist", serviceName: "CatalogService");
    }
}