using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence;

public class SeedData
{
    public static IEnumerable<User> Users => new List<User>
    {
        User.Create(
                guid: new Guid("be0cd669-237a-484d-b3cf-793e0ad1b0ea"),
                email: "adminsuper@gmail.com",
                firstName: "ADMIN SUPER",
                lastName: "USER",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "VN",
                emailConfirmed: true,
                tenantId: null,
                branchId: null,
                tenantCode: null),

        User.Create(
               guid: new Guid("65dad719-7368-4d9f-b623-f308299e9575"),
               email: "admin@gmail.com",
               firstName: "ADMIN",
               lastName: "USER",
               birthDay: DateTime.Parse("2004-09-17").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284890",
               passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
               image: null,
               country: "VN",
               emailConfirmed: true,
               tenantId: null,
               branchId: null,
               tenantCode: null),

        User.Create(
               guid: new Guid("e79d0b6f-af5a-4162-a6fd-8194d5a5f616"),
               email: "staff@gmail.com",
               firstName: "STAFF",
               lastName: "USER",
               birthDay: DateTime.Parse("2005-10-18").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284890",
               passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
               image: null,
               country: "VN",
               emailConfirmed: true,
               tenantId: null,
               branchId: null,
               tenantCode: null),

        User.Create(
               guid: new Guid("c3127b01-9101-4713-8e18-ae1f8f9ffd01"),
               email: "user@gmail.com",
               firstName: "USER",
               lastName: "USER",
               birthDay: DateTime.Parse("2006-11-19").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284890",
               passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
               image: null,
               country: "VN",
               emailConfirmed: true,
               tenantId: null,
               branchId: null,
               tenantCode: null),

        User.Create(
               guid: new Guid("8d8059c4-38b8-4f62-a776-4527e059b14a"),
               email: "foobar@gmail.com",
               firstName: "FOO",
               lastName: "BAR",
               birthDay: DateTime.Parse("2007-12-20").ToUniversalTime().AddHours(7),
               phoneNumber: "0333284890",
               passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
               image: null,
               country: "VN",
               emailConfirmed: true,
               tenantId: null,
               branchId: null,
               tenantCode: null),
    };

    public static IEnumerable<IdentityRole> Roles => new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = "3a0efb98-7841-4ff1-900c-e255ec60eb4f",
            Name = "ADMIN_SUPER",
            NormalizedName = "ADMIN_SUPER"
        },
        new IdentityRole
        {
            Id = "12d826a4-a9c0-471c-91f3-39b18993e0c1",
            Name = "ADMIN",
            NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
            Id = "12145c29-e918-4cee-b58c-e6fe2a66e560",
            Name = "STAFF",
            NormalizedName = "STAFF"
        },
        new IdentityRole
        {
            Id = "11118cf4-b9d1-430d-96c1-4e5272d6d112",
            Name = "USER",
            NormalizedName = "USER"
        },
    };

    public static IEnumerable<IdentityUserRole<string>> UserRoles => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "3a0efb98-7841-4ff1-900c-e255ec60eb4f", // SUPER_ADMIN
            UserId = "be0cd669-237a-484d-b3cf-793e0ad1b0ea" // SUPER ADMIN USER
        },
        new IdentityUserRole<string>
        {
            RoleId = "12d826a4-a9c0-471c-91f3-39b18993e0c1", // ADMIN
            UserId = "65dad719-7368-4d9f-b623-f308299e9575" // ADMIN USER
        },
        new IdentityUserRole<string>
        {
            RoleId = "12145c29-e918-4cee-b58c-e6fe2a66e560", // STAFF
            UserId = "e79d0b6f-af5a-4162-a6fd-8194d5a5f616" // STAFF USER
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "c3127b01-9101-4713-8e18-ae1f8f9ffd01" // USER USER
        },
        new IdentityUserRole<string>
        {
            RoleId = "11118cf4-b9d1-430d-96c1-4e5272d6d112", // USER
            UserId = "8d8059c4-38b8-4f62-a776-4527e059b14a" // FOO BAR
        },
    };
}
