

using YGZ.BuildingBlocks.Shared.Errors;
namespace YGZ.Keycloak.Domain.Core.Errors;

public static partial class Errors
{
    public static class Keycloak
    {
        public static Error CannotBeCreated = Error.BadRequest(code: "Keycloak.CannotBeCreated", message: "Keycloak cannot be created", serviceName: "KeycloakService");
        public static Error InvalidCredentials = Error.BadRequest(code: "Keycloak.InvalidCredentials", message: "Invalid credentials", serviceName: "KeycloakService");
    }
}