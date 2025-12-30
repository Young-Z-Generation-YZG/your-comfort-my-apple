using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Seeds;

public static class SeedCustomers
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

    public static IEnumerable<User> Customers
    {
        get
        {
            return new List<User>
            {
                User.Create(
                    guid: new Guid("c3127b01-9101-4713-8e18-ae1f8f9f1608"),
                    email: "lov3rinve146@gmail.com",
                    firstName: "Bách",
                    lastName: "Lê",
                    birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                    phoneNumber: "0333284890",
                    passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("c3127b01-9101-4713-8e18-ae1f8f9ffd01"),
                    email: "user@gmail.com",
                    firstName: "USER",
                    lastName: "USER",
                    birthDay: DateTime.Parse("2006-11-19").ToUniversalTime().AddHours(7),
                    phoneNumber: "0333284890",
                    passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("8d8059c4-38b8-4f62-a776-4527e059b14a"),
                    email: "foobar@gmail.com",
                    firstName: "FOO",
                    lastName: "BAR",
                    birthDay: DateTime.Parse("2007-12-20").ToUniversalTime().AddHours(7),
                    phoneNumber: "0333284890",
                    passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("a1b2c3d4-e5f6-4789-a0b1-c2d3e4f5a6b7"),
                    email: "nguyen.van.anh@gmail.com",
                    firstName: "Anh",
                    lastName: "Nguyễn Văn",
                    birthDay: DateTime.Parse("1995-03-15").ToUniversalTime().AddHours(7),
                    phoneNumber: "0912345678",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("b2c3d4e5-f6a7-4890-b1c2-d3e4f5a6b7c8"),
                    email: "tran.thi.lan@gmail.com",
                    firstName: "Lan",
                    lastName: "Trần Thị",
                    birthDay: DateTime.Parse("1998-07-22").ToUniversalTime().AddHours(7),
                    phoneNumber: "0923456789",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("c3d4e5f6-a7b8-4901-c2d3-e4f5a6b7c8d9"),
                    email: "le.duc.minh@gmail.com",
                    firstName: "Minh",
                    lastName: "Lê Đức",
                    birthDay: DateTime.Parse("1992-11-08").ToUniversalTime().AddHours(7),
                    phoneNumber: "0934567890",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("d4e5f6a7-b8c9-4012-d3e4-f5a6b7c8d9e0"),
                    email: "pham.thi.phuong@gmail.com",
                    firstName: "Phương",
                    lastName: "Phạm Thị",
                    birthDay: DateTime.Parse("1996-05-30").ToUniversalTime().AddHours(7),
                    phoneNumber: "0945678901",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("e5f6a7b8-c9d0-4123-e4f5-a6b7c8d9e0f1"),
                    email: "hoang.van.tuan@gmail.com",
                    firstName: "Tuấn",
                    lastName: "Hoàng Văn",
                    birthDay: DateTime.Parse("1994-09-14").ToUniversalTime().AddHours(7),
                    phoneNumber: "0956789012",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("f6a7b8c9-d0e1-4234-f5a6-b7c8d9e0f1a2"),
                    email: "vu.thi.huyen@gmail.com",
                    firstName: "Huyền",
                    lastName: "Vũ Thị",
                    birthDay: DateTime.Parse("1997-04-18").ToUniversalTime().AddHours(7),
                    phoneNumber: "0967890123",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("a7b8c9d0-e1f2-4345-a6b7-c8d9e0f1a2b3"),
                    email: "dang.van.hung@gmail.com",
                    firstName: "Hùng",
                    lastName: "Đặng Văn",
                    birthDay: DateTime.Parse("1993-08-25").ToUniversalTime().AddHours(7),
                    phoneNumber: "0978901234",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("b8c9d0e1-f2a3-4456-b7c8-d9e0f1a2b3c4"),
                    email: "bui.thi.thao@gmail.com",
                    firstName: "Thảo",
                    lastName: "Bùi Thị",
                    birthDay: DateTime.Parse("1999-12-05").ToUniversalTime().AddHours(7),
                    phoneNumber: "0989012345",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("c9d0e1f2-a3b4-4567-c8d9-e0f1a2b3c4d5"),
                    email: "do.van.cuong@gmail.com",
                    firstName: "Cường",
                    lastName: "Đỗ Văn",
                    birthDay: DateTime.Parse("1991-06-12").ToUniversalTime().AddHours(7),
                    phoneNumber: "0990123456",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("d0e1f2a3-b4c5-4678-d9e0-f1a2b3c4d5e6"),
                    email: "ho.thi.mai@gmail.com",
                    firstName: "Mai",
                    lastName: "Hồ Thị",
                    birthDay: DateTime.Parse("1996-02-28").ToUniversalTime().AddHours(7),
                    phoneNumber: "0901234567",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("e1f2a3b4-c5d6-4789-e0f1-a2b3c4d5e6f7"),
                    email: "ngo.van.quang@gmail.com",
                    firstName: "Quang",
                    lastName: "Ngô Văn",
                    birthDay: DateTime.Parse("1994-10-03").ToUniversalTime().AddHours(7),
                    phoneNumber: "0912345679",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("f2a3b4c5-d6e7-4890-f1a2-b3c4d5e6f7a8"),
                    email: "duong.thi.nga@gmail.com",
                    firstName: "Nga",
                    lastName: "Dương Thị",
                    birthDay: DateTime.Parse("1998-01-20").ToUniversalTime().AddHours(7),
                    phoneNumber: "0923456780",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("a3b4c5d6-e7f8-4901-a2b3-c4d5e6f7a8b9"),
                    email: "ly.van.son@gmail.com",
                    firstName: "Sơn",
                    lastName: "Lý Văn",
                    birthDay: DateTime.Parse("1992-07-17").ToUniversalTime().AddHours(7),
                    phoneNumber: "0934567891",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("b4c5d6e7-f8a9-4012-b3c4-d5e6f7a8b9c0"),
                    email: "vuong.thi.trang@gmail.com",
                    firstName: "Trang",
                    lastName: "Vương Thị",
                    birthDay: DateTime.Parse("1997-11-09").ToUniversalTime().AddHours(7),
                    phoneNumber: "0945678902",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("c5d6e7f8-a9b0-4123-c4d5-e6f7a8b9c0d1"),
                    email: "dao.van.dung@gmail.com",
                    firstName: "Dũng",
                    lastName: "Đào Văn",
                    birthDay: DateTime.Parse("1995-05-26").ToUniversalTime().AddHours(7),
                    phoneNumber: "0956789013",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("d6e7f8a9-b0c1-4234-d5e6-f7a8b9c0d1e2"),
                    email: "truong.thi.uyen@gmail.com",
                    firstName: "Uyên",
                    lastName: "Trương Thị",
                    birthDay: DateTime.Parse("1999-09-11").ToUniversalTime().AddHours(7),
                    phoneNumber: "0967890124",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]),
                User.Create(
                    guid: new Guid("e7f8a9b0-c1d2-4345-e6f7-a8b9c0d1e2f3"),
                    email: "phan.van.thanh@gmail.com",
                    firstName: "Thành",
                    lastName: "Phan Văn",
                    birthDay: DateTime.Parse("1993-03-23").ToUniversalTime().AddHours(7),
                    phoneNumber: "0978901235",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("f8a9b0c1-d2e3-4456-f7a8-b9c0d1e2f3a4"),
                    email: "le.thi.yen@gmail.com",
                    firstName: "Yến",
                    lastName: "Lê Thị",
                    birthDay: DateTime.Parse("1996-08-07").ToUniversalTime().AddHours(7),
                    phoneNumber: "0989012346",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("a9b0c1d2-e3f4-4567-a8b9-c0d1e2f3a4b5"),
                    email: "nguyen.van.phong@gmail.com",
                    firstName: "Phong",
                    lastName: "Nguyễn Văn",
                    birthDay: DateTime.Parse("1994-12-31").ToUniversalTime().AddHours(7),
                    phoneNumber: "0990123457",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
                User.Create(
                    guid: new Guid("b0c1d2e3-f4a5-4678-b9c0-d1e2f3a4b5c6"),
                    email: "tran.thi.hoa@gmail.com",
                    firstName: "Hoa",
                    lastName: "Trần Thị",
                    birthDay: DateTime.Parse("1997-06-14").ToUniversalTime().AddHours(7),
                    phoneNumber: "0901234568",
                    passwordHash: PasswordHashes["password"],
                    image: null,
                    country: "Vietnam",
                    emailConfirmed: true,
                    tenantId: TenantIds["YBZONE"],
                    branchId: BranchIds["YBZONE"]
                ),
            };
        }
    }
    
    public static IEnumerable<IdentityUserRole<string>> UserRolesCustomer => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "c3127b01-9101-4713-8e18-ae1f8f9f1608" // Bách Lê
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "c3127b01-9101-4713-8e18-ae1f8f9ffd01" // USER
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "8d8059c4-38b8-4f62-a776-4527e059b14a" // FOO BAR
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "a1b2c3d4-e5f6-4789-a0b1-c2d3e4f5a6b7" // Nguyễn Văn Anh
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "b2c3d4e5-f6a7-4890-b1c2-d3e4f5a6b7c8" // Trần Thị Lan
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "c3d4e5f6-a7b8-4901-c2d3-e4f5a6b7c8d9" // Lê Đức Minh
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "d4e5f6a7-b8c9-4012-d3e4-f5a6b7c8d9e0" // Phạm Thị Phương
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "e5f6a7b8-c9d0-4123-e4f5-a6b7c8d9e0f1" // Hoàng Văn Tuấn
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "f6a7b8c9-d0e1-4234-f5a6-b7c8d9e0f1a2" // Vũ Thị Huyền
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "a7b8c9d0-e1f2-4345-a6b7-c8d9e0f1a2b3" // Đặng Văn Hùng
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "b8c9d0e1-f2a3-4456-b7c8-d9e0f1a2b3c4" // Bùi Thị Thảo
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "c9d0e1f2-a3b4-4567-c8d9-e0f1a2b3c4d5" // Đỗ Văn Cường
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "d0e1f2a3-b4c5-4678-d9e0-f1a2b3c4d5e6" // Hồ Thị Mai
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "e1f2a3b4-c5d6-4789-e0f1-a2b3c4d5e6f7" // Ngô Văn Quang
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "f2a3b4c5-d6e7-4890-f1a2-b3c4d5e6f7a8" // Dương Thị Nga
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "a3b4c5d6-e7f8-4901-a2b3-c4d5e6f7a8b9" // Lý Văn Sơn
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "b4c5d6e7-f8a9-4012-b3c4-d5e6f7a8b9c0" // Vương Thị Trang
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "c5d6e7f8-a9b0-4123-c4d5-e6f7a8b9c0d1" // Đào Văn Dũng
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "d6e7f8a9-b0c1-4234-d5e6-f7a8b9c0d1e2" // Trương Thị Uyên
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "e7f8a9b0-c1d2-4345-e6f7-a8b9c0d1e2f3" // Phan Văn Thành
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "f8a9b0c1-d2e3-4456-f7a8-b9c0d1e2f3a4" // Lê Thị Yến
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "a9b0c1d2-e3f4-4567-a8b9-c0d1e2f3a4b5" // Nguyễn Văn Phong
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "b0c1d2e3-f4a5-4678-b9c0-d1e2f3a4b5c6" // Trần Thị Hoa
        },
    };
}
