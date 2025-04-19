
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Review
    {
        public static Error AddReviewFailure = Error.BadRequest(code: "Review.AddReviewFailure", message: "Add review failure", serviceName: "CatalogService");
    }
}