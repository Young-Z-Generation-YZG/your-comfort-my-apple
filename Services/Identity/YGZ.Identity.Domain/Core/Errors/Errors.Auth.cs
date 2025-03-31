

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class Auth
    {
        public static Error ExpiredToken = Error.BadRequest(code: "Auth.ExpiredToken", message: "Expired Otp", serviceName: "IdentityService");
        public static Error InvalidOtp = Error.BadRequest(code: "Auth.InvalidOtp", message: "Invalid Otp", serviceName: "IdentityService");
        public static Error InvalidToken = Error.BadRequest(code: "Auth.InvalidToken", message: "Invalid Token", serviceName: "IdentityService");
        public static Error ConfirmEmailVerificationFailure = Error.BadRequest(code: "Auth.ConfirmEmailVerificationFailure", message: "Failed to confirm email verification", serviceName: "IdentityService");
    }
}