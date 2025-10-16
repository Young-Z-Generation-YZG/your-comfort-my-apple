using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class Email
    {
        public static Error FailureToSendEmail = Error.BadRequest(code: "Email.FailureToSendEmail", message: "Failure to send email", serviceName: "IdentityService");
    }
}