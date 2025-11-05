

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DoesNotExist = Error.BadRequest(code: "User.DoesNotExist", message: "User does not exists", serviceName: "IdentityService");
        public static Error AlreadyExists = Error.BadRequest(code: "User.AlreadyExists", message: "User already exists", serviceName: "IdentityService");
        public static Error CannotBeCreated = Error.BadRequest(code: "User.CannotBeCreated", message: "User cannot be created", serviceName: "IdentityService");
        public static Error InvalidCredentials = Error.BadRequest(code: "User.InvalidCredentials", message: "Invalid credentials", serviceName: "IdentityService");
        public static Error CannotBeUpdated = Error.BadRequest(code: "User.CannotBeUpdated", message: "User cannot be updated", serviceName: "IdentityService");
        public static Error CannotResetPassword = Error.BadRequest(code: "User.CannotResetPassword", message: "User cannot reset password", serviceName: "IdentityService");
        public static Error CannotChangePassword = Error.BadRequest(code: "User.CannotChangePassword", message: "User cannot change password", serviceName: "IdentityService");
        public static Error CannotGetRoles = Error.BadRequest(code: "User.CannotGetRoles", message: "Cannot get user roles", serviceName: "IdentityService");
    }
}