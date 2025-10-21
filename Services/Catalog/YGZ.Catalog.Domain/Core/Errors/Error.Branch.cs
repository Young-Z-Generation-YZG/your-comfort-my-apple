using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Brancch
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Branch.DoesNotExist", message: "Branch does not exist", serviceName: "CatalogService");
    }
}