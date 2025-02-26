

using YGZ.BuildingBlocks.Shared.Errors;
namespace YGZ.Keycloak.Domain.Core.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DoesNotExist = Error.BadRequest(code: "User.DoesNotExist", message: "User does not exists", serviceName: "IdentityService");
        public static Error AlreadyExists = Error.BadRequest(code: "User.AlreadyExists", message: "User already exists", serviceName: "IdentityService");
        public static Error CannotBeCreated = Error.BadRequest(code: "User.CannotBeCreated", message: "User cannot be created", serviceName: "IdentityService");
        public static Error InvalidCredentials = Error.BadRequest(code: "User.InvalidCredentials", message: "Invalid credentials", serviceName: "IdentityService");
    }
}