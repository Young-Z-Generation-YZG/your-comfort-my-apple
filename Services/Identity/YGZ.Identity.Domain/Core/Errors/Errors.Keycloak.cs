

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class Keycloak
    {
        public static Error UserAlreadyExist = Error.BadRequest(code: "Keycloak.UserAlreadyExist", message: "User already exist", serviceName: "KeycloakService");
        public static Error CannotBeCreated = Error.BadRequest(code: "Keycloak.CannotBeCreated", message: "User cannot be created", serviceName: "KeycloakService");
        public static Error InvalidCredentials = Error.BadRequest(code: "Keycloak.InvalidCredentials", message: "Invalid credentials", serviceName: "KeycloakService");
    }
}