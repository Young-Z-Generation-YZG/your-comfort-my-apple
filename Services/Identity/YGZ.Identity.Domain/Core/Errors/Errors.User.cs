

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DoesNotExist = Error.BadRequest(code: "User.DoesNotExist", message: "User does not exists", serivceName: "IdentityService");
        public static Error AlreadyExists = Error.BadRequest(code: "User.AlreadyExists", message: "User already exists", serivceName: "IdentityService");
        public static Error CannotBeCreated = Error.BadRequest(code: "User.CannotBeCreated", message: "User cannot be created", serivceName: "IdentityService");
        public static Error InvalidPassword = Error.BadRequest(code: "User.InvalidPassword", message: "Invalid password", serivceName: "IdentityService");
    }
}
