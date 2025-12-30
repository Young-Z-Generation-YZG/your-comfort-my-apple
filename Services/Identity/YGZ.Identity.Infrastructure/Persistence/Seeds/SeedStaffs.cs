using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Seeds;

public static class SeedStaffs
{
    public static Dictionary<string, string> TenantIds => new Dictionary<string, string>
    {
        { "YBZONE", "664355f845e56534956be32b" },
        { "HCM_TD_KVC_1060", "690e034dff79797b05b3bc89" },
        { "HCM_Q1_CMT8_92", "690e034dff79797b05b3bc90" },
        { "HCM_Q9_LVV_123", "690e034dff79797b05b3bc91" },
    };

    public static Dictionary<string, string> BranchIds => new Dictionary<string, string>
    {
        { "YBZONE", "664357a235e84033bbd0e6b6" },
        { "HCM_TD_KVC_1060", "690e034dff79797b05b3bc88" },
        { "HCM_Q1_CMT8_92", "690e034dff79797b05b3bc12" },
        { "HCM_Q9_LVV_123", "690e034dff79797b05b3bc13" },
    };

    public static Dictionary<string, string> PasswordHashes => new Dictionary<string, string>
    {
        { "password", "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==" },
    };

    public static IEnumerable<User> UsersTenant_YBZONE
    {
        get
        {
            var adminSuper = User.Create(
               guid: new Guid("be0cd669-237a-484d-b3cf-793e0ad1b0ea"),
               email: "adminsuper@ybzone.com",
               firstName: "ADMIN SUPER",
               lastName: "USER",
               birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["YBZONE"],
               branchId: BranchIds["YBZONE"]);

            var admin1 = User.Create(
               guid: new Guid("0dbb05e1-97be-4a38-9bf0-478005b48b62"),
               email: "admin1@ybzone.com",
               firstName: "ADMIN 1",
               lastName: "USER",
               birthDay: DateTime.Parse("2004-09-17").ToUniversalTime().AddHours(7),
               phoneNumber: "0333394890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["YBZONE"],
               branchId: BranchIds["YBZONE"]);



            return new List<User>
            {
                adminSuper,
                admin1,
            };
        }
    }

    public static IEnumerable<IdentityUserRole<string>> UserRoles_YBZONE => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "3a0efb98-7841-4ff1-900c-e255ec60eb4f", // ADMIN_SUPER_YBZONE
            UserId = "be0cd669-237a-484d-b3cf-793e0ad1b0ea" // ADMIN SUPER YBZONE
        },
        new IdentityUserRole<string>
        {
            RoleId = "12d826a4-a9c0-471c-91f3-39b18993e0c1", // ADMIN_YBZONE
            UserId = "0dbb05e1-97be-4a38-9bf0-478005b48b62" // ADMIN 1
        }
    };

    public static IEnumerable<User> UsersTenant_HCM_TD_KVC_1060
    {
        get
        {
            var admin1 = User.Create(
               guid: new Guid("65dad719-7368-4d9f-b623-f308299e9575"),
               email: "admin1@hcm-td-kvc-1060.com",
               firstName: "ADMIN 1",
               lastName: "USER",
               birthDay: DateTime.Parse("2004-09-17").ToUniversalTime().AddHours(7),
               phoneNumber: "0333394890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_TD_KVC_1060"],
               branchId: BranchIds["HCM_TD_KVC_1060"]);

            var staff1 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f616"),
               email: "staff1@hcm-td-kvc-1060.com",
               firstName: "STAFF 1",
               lastName: "USER",
               birthDay: DateTime.Parse("2005-10-18").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_TD_KVC_1060"],
               branchId: BranchIds["HCM_TD_KVC_1060"]);

            var staff2 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f617"),
               email: "staff2@hcm-td-kvc-1060.com",
               firstName: "STAFF 2",
               lastName: "USER",
               birthDay: DateTime.Parse("2006-11-19").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284891",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_TD_KVC_1060"],
               branchId: BranchIds["HCM_TD_KVC_1060"]);

            var staff3 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f618"),
               email: "staff3@hcm-td-kvc-1060.com",
               firstName: "STAFF 3",
               lastName: "USER",
               birthDay: DateTime.Parse("2007-12-20").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284892",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_TD_KVC_1060"],
               branchId: BranchIds["HCM_TD_KVC_1060"]);

            var staff4 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f619"),
               email: "staff4@hcm-td-kvc-1060.com",
               firstName: "STAFF 4",
               lastName: "USER",
               birthDay: DateTime.Parse("2008-01-21").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284893",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_TD_KVC_1060"],
               branchId: BranchIds["HCM_TD_KVC_1060"]);

            var staff5 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f620"),
               email: "staff5@hcm-td-kvc-1060.com",
               firstName: "STAFF 5",
               lastName: "USER",
               birthDay: DateTime.Parse("2009-02-22").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284894",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_TD_KVC_1060"],
               branchId: BranchIds["HCM_TD_KVC_1060"]);

            return new List<User>
            {
                admin1,
                staff1,
                staff2,
                staff3,
                staff4,
                staff5
            };
        }
    }

    public static IEnumerable<IdentityUserRole<string>> UserRoles_HCM_TD_KVC_1060 => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a33e561", // ADMIN_BRANCH
            UserId = "65dad719-7368-4d9f-b623-f308299e9575" // ADMIN 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f616" // STAFF 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f617" // STAFF 2
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f618" // STAFF 3
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f619" // STAFF 4
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f620" // STAFF 5
        }
    };

    public static IEnumerable<User> UsersTenant_HCM_Q1_CMT8_92
    {
        get
        {
            var admin1 = User.Create(
               guid: new Guid("65dad719-7368-4d9f-b623-f308299e9556"),
               email: "admin1@hcm-q1-cmt8-92.com",
               firstName: "ADMIN 1",
               lastName: "USER",
               birthDay: DateTime.Parse("2004-09-17").ToUniversalTime().AddHours(7),
               phoneNumber: "0333494890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q1_CMT8_92"],
               branchId: BranchIds["HCM_Q1_CMT8_92"]);

            var staff1 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f655"),
               email: "staff1@hcm-q1-cmt8-92.com",
               firstName: "STAFF 1",
               lastName: "USER",
               birthDay: DateTime.Parse("2005-10-18").ToUniversalTime().AddHours(7),
               phoneNumber: "0333384890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q1_CMT8_92"],
               branchId: BranchIds["HCM_Q1_CMT8_92"]);

            var staff2 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f654"),
               email: "staff2@hcm-q1-cmt8-92.com",
               firstName: "STAFF 2",
               lastName: "USER",
               birthDay: DateTime.Parse("2006-11-19").ToUniversalTime().AddHours(7),
               phoneNumber: "0333384891",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q1_CMT8_92"],
               branchId: BranchIds["HCM_Q1_CMT8_92"]);

            var staff3 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f653"),
               email: "staff3@hcm-q1-cmt8-92.com",
               firstName: "STAFF 3",
               lastName: "USER",
               birthDay: DateTime.Parse("2007-12-20").ToUniversalTime().AddHours(7),
               phoneNumber: "0333384892",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q1_CMT8_92"],
               branchId: BranchIds["HCM_Q1_CMT8_92"]);

            var staff4 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f652"),
               email: "staff4@hcm-q1-cmt8-92.com",
               firstName: "STAFF 4",
               lastName: "USER",
               birthDay: DateTime.Parse("2008-01-21").ToUniversalTime().AddHours(7),
               phoneNumber: "0333384893",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q1_CMT8_92"],
               branchId: BranchIds["HCM_Q1_CMT8_92"]);

            var staff5 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f651"),
               email: "staff5@hcm-q1-cmt8-92.com",
               firstName: "STAFF 5",
               lastName: "USER",
               birthDay: DateTime.Parse("2009-02-22").ToUniversalTime().AddHours(7),
               phoneNumber: "0333384894",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q1_CMT8_92"],
               branchId: BranchIds["HCM_Q1_CMT8_92"]);

            return new List<User>
            {
                admin1,
                staff1,
                staff2,
                staff3,
                staff4,
                staff5
            };
        }
    }

    public static IEnumerable<IdentityUserRole<string>> UserRoles_HCM_Q1_CMT8_92 => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a33e561", // ADMIN_BRANCH
            UserId = "65dad719-7368-4d9f-b623-f308299e9556" // ADMIN 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f655" // STAFF 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f654" // STAFF 2
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f653" // STAFF 3
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f652" // STAFF 4
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f651" // STAFF 5
        }
    };

    public static IEnumerable<User> UsersTenant_HCM_Q9_LVV_123
    {
        get
        {
            var admin1 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f120"),
               email: "admin1@hcm-q9-lvv-123.com",
               firstName: "ADMIN 1",
               lastName: "USER",
               birthDay: DateTime.Parse("2004-09-17").ToUniversalTime().AddHours(7),
               phoneNumber: "0333594890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q9_LVV_123"],
               branchId: BranchIds["HCM_Q9_LVV_123"]);

            var staff1 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f121"),
               email: "staff1@hcm-q9-lvv-123.com",
               firstName: "STAFF 1",
               lastName: "USER",
               birthDay: DateTime.Parse("2005-10-18").ToUniversalTime().AddHours(7),
               phoneNumber: "0333484890",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q9_LVV_123"],
               branchId: BranchIds["HCM_Q9_LVV_123"]);

            var staff2 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f122"),
               email: "staff2@hcm-q9-lvv-123.com",
               firstName: "STAFF 2",
               lastName: "USER",
               birthDay: DateTime.Parse("2006-11-19").ToUniversalTime().AddHours(7),
               phoneNumber: "0333484891",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q9_LVV_123"],
               branchId: BranchIds["HCM_Q9_LVV_123"]);

            var staff3 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f123"),
               email: "staff3@hcm-q9-lvv-123.com",
               firstName: "STAFF 3",
               lastName: "USER",
               birthDay: DateTime.Parse("2007-12-20").ToUniversalTime().AddHours(7),
               phoneNumber: "0333484892",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q9_LVV_123"],
               branchId: BranchIds["HCM_Q9_LVV_123"]);

            var staff4 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f124"),
               email: "staff4@hcm-q9-lvv-123.com",
               firstName: "STAFF 4",
               lastName: "USER",
               birthDay: DateTime.Parse("2008-01-21").ToUniversalTime().AddHours(7),
               phoneNumber: "0333484893",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q9_LVV_123"],
               branchId: BranchIds["HCM_Q9_LVV_123"]);

            var staff5 = User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f125"),
               email: "staff5@hcm-q9-lvv-123.com",
               firstName: "STAFF 5",
               lastName: "USER",
               birthDay: DateTime.Parse("2009-02-22").ToUniversalTime().AddHours(7),
               phoneNumber: "0333484894",
               passwordHash: PasswordHashes["password"],
               image: null,
               country: "Vietnam",
               emailConfirmed: true,
               tenantId: TenantIds["HCM_Q9_LVV_123"],
               branchId: BranchIds["HCM_Q9_LVV_123"]);

            return new List<User>
            {
                admin1,
                staff1,
                staff2,
                staff3,
                staff4,
                staff5
            };
        }
    }

    public static IEnumerable<IdentityUserRole<string>> UserRoles_HCM_Q9_LVV_123 => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a33e561", // ADMIN_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f120" // ADMIN 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f121" // STAFF 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f122" // STAFF 2
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f123" // STAFF 3
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f124" // STAFF 4
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF_BRANCH
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f125" // STAFF 5
        }
    };
}
