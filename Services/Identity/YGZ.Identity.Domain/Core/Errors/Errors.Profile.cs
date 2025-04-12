
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Identity.Domain.Core.Errors;

public static partial class Errors
{
    public static class Profile
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Profile.DoesNotExist", message: "Profile does not exists", serviceName: "IdentityService");
    }
}