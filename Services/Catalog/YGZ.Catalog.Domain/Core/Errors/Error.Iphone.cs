
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Iphone
    {
        public static Error ModelDoesNotExist = Error.BadRequest(code: "Iphone.ModelDoesNotExist", message: "Iphone model does not exist", serviceName: "CatalogService");
    }
}