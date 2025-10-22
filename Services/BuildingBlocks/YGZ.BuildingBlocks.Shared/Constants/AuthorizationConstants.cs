

namespace YGZ.BuildingBlocks.Shared.Constants;

public static class AuthorizationConstants
{
    public static class Resources
    {
        public const string RESOURCE_USERS = nameof(RESOURCE_USERS);
    }

    public static class Scopes
    {
        public const string READ_OWN = nameof(READ_OWN);
        public const string UPDATE_OWN = nameof(UPDATE_OWN);
        public const string DELETE_OWN = nameof(DELETE_OWN);
    }

    public static class Policies
    {
        public const string REQUIRE_AUTHENTICATION = nameof(REQUIRE_AUTHENTICATION);
    }

    public static class Roles
    {
        public const string USER = nameof(USER);
        public const string STAFF = nameof(STAFF);
    }
}
