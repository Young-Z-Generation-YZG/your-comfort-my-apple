using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Ordering.Domain.Core.Errors;

public static partial class Errors
{
    public static class Common
    {
        public static Error AddFailure = Error.BadRequest(code: "Common.AddFailure", message: "Add failure", serviceName: "OrderingService");
        public static Error UpdateFailure = Error.BadRequest(code: "Common.UpdateFailure", message: "Update failure", serviceName: "OrderingService");
    }
}