
using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence;

public class SeedData
{
    public static IEnumerable<User> Users => new List<User>
    {
        User.Create(
                guid: new Guid("c77a114d-2dd8-4746-806b-030878618282"),
                email: "superadmin@gmail.com",
                firstName: "SUPER ADMIN",
                lastName: "CLIENT",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "Vietnam", emailConfirmed: true),

        User.Create(
                guid: new Guid("a3f9cda2-ac4f-4f1b-9103-e24b586f98ae"),
                email: "admin@gmail.com",
                firstName: "ADMIN",
                lastName: "CLIENT",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "Vietnam", emailConfirmed: true),

        User.Create(
                guid: new Guid("3edc9527-4f3b-4619-b4b2-18025e143eda"),
                email: "staff@gmail.com",
                firstName: "STAFF",
                lastName: "CLIENT",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "Vietnam", emailConfirmed: true),

        User.Create(
                guid: new Guid("068b089e-588b-4a40-9dc7-41474dcdbfa8"),
                email: "user@gmail.com",
                firstName: "USER",
                lastName: "CLIENT",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "Vietnam", emailConfirmed: true),

        User.Create(
                guid: new Guid("919d5784-2275-451f-9c4c-af80fb0b5cb3"),
                email: "foobar@gmail.com",
                firstName: "FOO",
                lastName: "BAR",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "Vietnam", emailConfirmed: true),

        User.Create(
                guid: new Guid("ed04b044-86de-475f-9122-d9807897f969"),
                email: "lov3rinve146@gmail.com",
                firstName: "Bach",
                lastName: "Le",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "Vietnam", emailConfirmed: true),

    };

    public static IEnumerable<IdentityRole> Roles => new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = "b7852d28-8e40-4e53-85cd-9906b37eb31a",
            Name = "[ROLE]:SUPER_ADMIN",
            NormalizedName = "[ROLE]:SUPER_ADMIN"
        },
        new IdentityRole
        {
            Id = "b3e0fdd9-8470-42e2-a949-369711161693",
            Name = "[ROLE]:ADMIN",
            NormalizedName = "[ROLE]:ADMIN"
        },
        new IdentityRole
        {
            Id = "80434e6e-4226-4dc0-93e1-8dc0eb19f214",
            Name = "[ROLE]:STAFF",
            NormalizedName = "[ROLE]:STAFF"
        },
        new IdentityRole
        {
            Id = "5bdc5c4f-6b92-4db1-b707-1b05ee0ea568",
            Name = "[ROLE]:USER",
            NormalizedName = "[ROLE]:USER"
        },
    };

    public static IEnumerable<IdentityUserRole<string>> UserRoles => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "b7852d28-8e40-4e53-85cd-9906b37eb31a", // SUPER_ADMIN
            UserId = "c77a114d-2dd8-4746-806b-030878618282" // SUPER ADMIN CLIENT
        },
        new IdentityUserRole<string>
        {
            RoleId = "b3e0fdd9-8470-42e2-a949-369711161693", // ADMIN
            UserId = "a3f9cda2-ac4f-4f1b-9103-e24b586f98ae" // ADMIN CLIENT
        },
        new IdentityUserRole<string>
        {
            RoleId = "80434e6e-4226-4dc0-93e1-8dc0eb19f214", // STAFF
            UserId = "3edc9527-4f3b-4619-b4b2-18025e143eda" // STAFF CLIENT
        },
        new IdentityUserRole<string>
        {
            RoleId = "b3e0fdd9-8470-42e2-a949-369711161693", // USER
            UserId = "068b089e-588b-4a40-9dc7-41474dcdbfa8" // USER CLIENT
        },
        new IdentityUserRole<string>
        {
            RoleId = "b3e0fdd9-8470-42e2-a949-369711161693", // USER
            UserId = "919d5784-2275-451f-9c4c-af80fb0b5cb3" // FOO BAR
        },
        new IdentityUserRole<string>
        {
            RoleId = "b3e0fdd9-8470-42e2-a949-369711161693", // USER
            UserId = "ed04b044-86de-475f-9122-d9807897f969" // Bach Le
        },
    };
}
