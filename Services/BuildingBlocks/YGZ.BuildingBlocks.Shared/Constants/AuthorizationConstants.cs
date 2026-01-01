

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
        public const string CREATE_ANY = nameof(CREATE_ANY);
        public const string READ_ANY = nameof(READ_ANY);
        public const string UPDATE_ANY = nameof(UPDATE_ANY);
        public const string DELETE_ANY = nameof(DELETE_ANY);

    }

    public static class Policies
    {
        public const string REQUIRE_AUTHENTICATION = nameof(REQUIRE_AUTHENTICATION);
        public const string R__ADMIN_SUPER___RS__ALL = nameof(R__ADMIN_SUPER___RS__ALL);
        public const string GetUsers = nameof(GetUsers);
    }

    public static class Roles
    {
        public const string ADMIN_SUPER_YBZONE = nameof(ADMIN_SUPER_YBZONE);
        public const string ADMIN_YBZONE = nameof(ADMIN_YBZONE);
        public const string ADMIN_BRANCH = nameof(ADMIN_BRANCH);
        public const string STAFF_BRANCH = nameof(STAFF_BRANCH);
        public const string USER = nameof(USER);
    }
}
