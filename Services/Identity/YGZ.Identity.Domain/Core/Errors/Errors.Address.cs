

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class Address
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Address.DoesNotExist", message: "Address does not exists", serviceName: "IdentityService");
        public static Error CanNotAdd = Error.BadRequest(code: "Address.CanNotAdd", message: "Can not add new address", serviceName: "IdentityService");
        public static Error NotFound = Error.BadRequest(code: "Address.NotFound", message: "Address not found", serviceName: "IdentityService");
        public static Error CanNotUpdate = Error.BadRequest(code: "Address.CanNotUpdate", message: "Can not update address", serviceName: "IdentityService");
        public static Error CanNotDelete = Error.BadRequest(code: "Address.CanNotDelete", message: "Can not delete address", serviceName: "IdentityService");
    }
}