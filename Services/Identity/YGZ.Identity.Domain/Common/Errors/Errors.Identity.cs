
namespace YGZ.Identity.Domain.Common.Errors;

public static partial class Errors
{
    public static class Identity
    {
        public static Error UserDoesNotExist = Error.BadRequest(code: "Identity.UserDoesNotExist", message: "User does not Exists");
        public static Error UserAlreadyExists = Error.BadRequest(code: "Identity.UserAlreadyExists", message: "User already Exists");
        public static Error UserCannotBeCreated = Error.BadRequest(code: "Identity.UserCannotBeCreated", message: "User cannot be created");
    }
}
