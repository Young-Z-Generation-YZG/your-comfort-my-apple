

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class Keycloak
    {
        public static Error LoginFailed = Error.BadRequest(code: "Keycloak.Login", message: "Login failed", serviceName: "KeycloakService");
        public static Error UserAlreadyExist = Error.BadRequest(code: "Keycloak.UserAlreadyExist", message: "User already exist", serviceName: "KeycloakService");
        public static Error UserNotFound = Error.BadRequest(code: "Keycloak.UserNotFound", message: "User not found", serviceName: "KeycloakService");
        public static Error CannotBeCreated = Error.BadRequest(code: "Keycloak.CannotBeCreated", message: "User cannot be created", serviceName: "KeycloakService");
        public static Error CannotAssignRole = Error.BadRequest(code: "Keycloak.CannotAssignRole", message: "Cannot assign role", serviceName: "KeycloakService");
        public static Error InvalidCredentials = Error.BadRequest(code: "Keycloak.InvalidCredentials", message: "Invalid credentials", serviceName: "KeycloakService");
        public static Error EmailVerificationFailed = Error.BadRequest(code: "Keycloak.EmailVerificationFailed", message: "Email verification failed", serviceName: "KeycloakService");
        public static Error SendEmailResetPasswordFailed = Error.BadRequest(code: "Keycloak.SendEmailResetPasswordFailed", message: "Send email reset password failed", serviceName: "KeycloakService");
        public static Error ResetPasswordFailed = Error.BadRequest(code: "Keycloak.ResetPasswordFailed", message: "Reset password failed", serviceName: "KeycloakService");
        public static Error ChangePasswordFailed = Error.BadRequest(code: "Keycloak.ChangePasswordFailed", message: "Change password failed", serviceName: "KeycloakService");
    }
}