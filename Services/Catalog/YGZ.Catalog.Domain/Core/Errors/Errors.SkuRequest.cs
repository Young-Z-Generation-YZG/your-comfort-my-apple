using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class SkuRequest
    {
        public static readonly Error NotFound = Error.NotFound(
            code: "SkuRequest.NotFound",
            message: "The SKU request with the specified identifier was not found.",
            serviceName: "CatalogService");

        public static readonly Error InvalidState = Error.Validation(
            code: "SkuRequest.InvalidState",
            message: "The SKU request is in an invalid state for this operation.",
            serviceName: "CatalogService");

        public static readonly Error InvalidTransition = Error.Validation(
            code: "SkuRequest.InvalidTransition",
            message: "Can only Approve or Reject pending requests.",
            serviceName: "CatalogService");

        public static readonly Error OperationFailed = Error.OperationFailed(
            code: "SkuRequest.OperationFailed",
            message: "The operation failed due to a domain error.",
            serviceName: "CatalogService");

        public static readonly Error SkuNotFound = Error.NotFound(
            code: "SkuRequest.SkuNotFound",
            message: "The SKU with the specified identifier was not found.",
            serviceName: "CatalogService");

        public static readonly Error InsufficientStock = Error.Validation(
            code: "SkuRequest.InsufficientStock",
            message: "Request quantity must be less than available stock.",
            serviceName: "CatalogService");
    }
}
