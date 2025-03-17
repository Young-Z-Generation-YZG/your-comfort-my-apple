
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence;

public class SeedData
{
    public static IEnumerable<User> Users => new List<User>
    {
        User.Create("lov3rinve146@gmail.com", "AQAAAAIAAYagAAAAEBFPYp8E9ls0cJ3mYC/mUbYy357lEBhrrfjrME7XKrjR2Qt7ydW/yW2UjLRdh85ggQ==", "Bach", "Le"),
        User.Create("user@gmail.com", "AQAAAAIAAYagAAAAEFTahSe/6Ime9FVOyF2UU1bwvOAMHWvBz6uuezjSUokY/iXpzOd7ZPkFBmTFI4soxg==", "USER", "CLIENT"),
        User.Create("admin@gmail.com", "AQAAAAIAAYagAAAAEI8p6o3qktBhloGkgcUIAx7k/M7aW5QeZ0mL1RpeAZbXdhSJSXNbALVUA0Qr2SYfHQ==", "ADMIN", "CLIENT"),
    };
}
