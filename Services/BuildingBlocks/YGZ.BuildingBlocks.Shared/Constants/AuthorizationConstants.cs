

namespace YGZ.BuildingBlocks.Shared.Constants;

public static class AuthorizationConstants
{
    public static class Resources
    {
        public const string RESOURCE_USERS = nameof(RESOURCE_USERS);
        public const string RESOURCE_ORDERS = nameof(RESOURCE_ORDERS);
    }

    public static class Scopes
    {
        public const string ALL = nameof(ALL);
        public const string CREATE_OWN = nameof(CREATE_OWN);
        public const string READ_OWN = nameof(READ_OWN);
        public const string UPDATE_OWN = nameof(UPDATE_OWN);
        public const string DELETE_OWN = nameof(DELETE_OWN);
        public const string CREATE_ALL = nameof(CREATE_ALL);
        public const string READ_ALL = nameof(READ_ALL);
        public const string UPDATE_ALL = nameof(UPDATE_ALL);
        public const string DELETE_ALL = nameof(DELETE_ALL);

    }

    public static class Policies
    {
        public const string REQUIRE_AUTHENTICATION = nameof(REQUIRE_AUTHENTICATION);
        public const string R__ADMIN_SUPER___RS__ALL = nameof(R__ADMIN_SUPER___RS__ALL);
    }

    public static class Roles
    {
        public const string ADMIN_SUPER = nameof(ADMIN_SUPER);
        public const string STAFF = nameof(STAFF);
        public const string USER = nameof(USER);
    }
}
