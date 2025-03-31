
using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence;

public class SeedData
{
    public static IEnumerable<User> Users => new List<User>
    {
        User.Create(
                guid: new Guid("ed04b044-86de-475f-9122-d9807897f969"),
                email: "lov3rinve146@gmail.com",
                firstName: "Bach",
                lastName: "Le",
                birthDay: DateTime.Parse("2003-08-16").ToUniversalTime().AddHours(7),
                phoneNumber: "0333284890",
                passwordHash: "AQAAAAIAAYagAAAAEKktVj/8xY/X/VzgaQKWFICiavT1Zh894avTa8W1TDnD5xm9wjAohg3UuLPdL7QEWQ==",
                image: null,
                country: "Vietnam"),
        //User.Create(new Guid("b3e0fdd9-8470-42e2-a949-369711161693"), "user@gmail.com", "AQAAAAIAAYagAAAAEFTahSe/6Ime9FVOyF2UU1bwvOAMHWvBz6uuezjSUokY/iXpzOd7ZPkFBmTFI4soxg==", "USER", "CLIENT"),
        //User.Create(new Guid("b3e0fdd9-8470-42e2-a949-369711161693"), "admin@gmail.com", "AQAAAAIAAYagAAAAEI8p6o3qktBhloGkgcUIAx7k/M7aW5QeZ0mL1RpeAZbXdhSJSXNbALVUA0Qr2SYfHQ==", "ADMIN", "CLIENT"),
    };

    public static IEnumerable<IdentityRole> Roles => new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = "b3e0fdd9-8470-42e2-a949-369711161693",
            Name = "[ROLE]:USER",
            NormalizedName = "[ROLE]:USER"
        },
    };

    public static IEnumerable<IdentityUserRole<string>> UserRoles => new List<IdentityUserRole<string>>
    {
        new IdentityUserRole<string>
        {
            RoleId = "b3e0fdd9-8470-42e2-a949-369711161693",
            UserId = "ed04b044-86de-475f-9122-d9807897f969"
        }
    };
}
