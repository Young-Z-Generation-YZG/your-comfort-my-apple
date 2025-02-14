
namespace YGZ.Identity.Domain.Authorizations;

public static partial class Policies
{
    public static partial class ApiScope
    {
        public const string ReadAccess = "read_access";
        public const string WriteAccess = "write_access";
        public const string UpdateAccess = "update_access";
        public const string DeleteAccess = "delete_access";
    }

    public const string UpdateOwnProfile = "update_own_profile";
}
