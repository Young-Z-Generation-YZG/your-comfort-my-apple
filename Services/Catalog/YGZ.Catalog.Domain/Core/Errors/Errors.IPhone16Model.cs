
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class IPhone16Model
    {
        public static Error NotFound = Error.BadRequest(code: "IPhone16Model.NotFound", message: "IPhone16Model not found", serviceName: "CatalogService");
        public static Error UpdatedFailure = Error.BadRequest(code: "IPhone16Model.UpdatedFailure", message: "IPhone16Model failure to update", serviceName: "CatalogService");
        public static Error UpdateReviewFailure = Error.BadRequest(code: "IPhone16Model.UpdateReviewFailure", message: "IPhone16Model failure to update review", serviceName: "CatalogService");
    }
}