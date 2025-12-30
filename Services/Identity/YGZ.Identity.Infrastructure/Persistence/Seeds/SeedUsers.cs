using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Seeds;

public static class SeedUsers
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
               firstName: "ADMIN WAREHOUSE",
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

    public static IEnumerable<User> UsersTenant_HCM_TD_KVC_1060
    {
        get
        {
            var admin1 = User.Create(
               guid: new Guid("0dbb05e1-97be-4a38-9bf0-478005b48b62"),
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
               guid: new Guid("6cb4412f-8241-4b29-b56a-f3911977c1e0"),
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
               guid: new Guid("6da1b40f-b9c6-4934-8479-6ffb6a92b354"),
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
               guid: new Guid("59fccd42-e244-496b-bb61-39d8898abb89"),
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
               guid: new Guid("43b7a6d9-c649-4dde-b6c5-7b6f0ba892b2"),
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
               guid: new Guid("d7438b42-cfd6-4078-a8d2-b68da4309a47"),
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

    public static IEnumerable<User> UsersTenant_HCM_Q1_CMT8_92
    {
        get
        {
            var admin1 = User.Create(
               guid: new Guid("1ecc16f2-a8cf-4b49-ac01-589116c59c73"),
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
               guid: new Guid("7dc55230-9352-5c3a-c67b-04a22888d2f1"),
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
               guid: new Guid("7eb2c520-cad7-5a45-958a-7aac7ba03c465"),
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
               guid: new Guid("6aafde53-f355-5a7c-cc7c-4ae99a9bcc9a"),
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
               guid: new Guid("54c8b7ea-d75a-5ede-c7d6-8c7a0cb9a3c3"),
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
               guid: new Guid("e8549c53-dae7-5b8f-d8e3-c7d9e1b0f1b5"),
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

    public static IEnumerable<IdentityUserRole<string>> UserRoles_HCM_TD_KVC_1060 => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "12d826a4-a9c0-471c-91f3-39b18993e0c1", // ADMIN
            UserId = "0dbb05e1-97be-4a38-9bf0-478005b48b62" // ADMIN 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "6cb4412f-8241-4b29-b56a-f3911977c1e0" // STAFF 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "6da1b40f-b9c6-4934-8479-6ffb6a92b354" // STAFF 2
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "59fccd42-e244-496b-bb61-39d8898abb89" // STAFF 3
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "43b7a6d9-c649-4dde-b6c5-7b6f0ba892b2" // STAFF 4
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "d7438b42-cfd6-4078-a8d2-b68da4309a47" // STAFF 5
        }
    };

    public static IEnumerable<IdentityUserRole<string>> UserRoles_HCM_Q1_CMT8_92 => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "12d826a4-a9c0-471c-91f3-39b18993e0c1", // ADMIN
            UserId = "1ecc16f2-a8cf-4b49-ac01-589116c59c73" // ADMIN 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "7dc55230-9352-5c3a-c67b-04a22888d2f1" // STAFF 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "7eb2c520-cad7-5a45-958a-7aac7ba03c465" // STAFF 2
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "6aafde53-f355-5a7c-cc7c-4ae99a9bcc9a" // STAFF 3
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "54c8b7ea-d75a-5ede-c7d6-8c7a0cb9a3c3" // STAFF 4
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "e8549c53-dae7-5b8f-d8e3-c7d9e1b0f1b5" // STAFF 5
        }
    };

    public static IEnumerable<User> UsersTenant_HCM_Q9_LVV_123
    {
        get
        {
            var admin1 = User.Create(
               guid: new Guid("2fdd27a3-b9da-5c5a-bd12-69a227d6ad84"),
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
               guid: new Guid("8ed66341-a463-6d4b-d78c-15b33999e3a2"),
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
               guid: new Guid("8fc3d631-be48-6b56-069d-8bdd8cb14d576"),
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
               guid: new Guid("7bbaef64-a466-6c8d-dd8d-5bfaabacddab"),
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
               guid: new Guid("65d9c8fb-e86b-6fef-d8e7-9d8a1dca4d4d"),
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
               guid: new Guid("f965ad64-ebf8-6c9a-e9f4-d8eaf2c1a2c6"),
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
            RoleId = "12d826a4-a9c0-471c-91f3-39b18993e0c1", // ADMIN
            UserId = "2fdd27a3-b9da-5c5a-bd12-69a227d6ad84" // ADMIN 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "8ed66341-a463-6d4b-d78c-15b33999e3a2" // STAFF 1
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "8fc3d631-be48-6b56-069d-8bdd8cb14d576" // STAFF 2
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "7bbaef64-a466-6c8d-dd8d-5bfaabacddab" // STAFF 3
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "65d9c8fb-e86b-6fef-d8e7-9d8a1dca4d4d" // STAFF 4
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "f965ad64-ebf8-6c9a-e9f4-d8eaf2c1a2c6" // STAFF 5
        }
    };
}
