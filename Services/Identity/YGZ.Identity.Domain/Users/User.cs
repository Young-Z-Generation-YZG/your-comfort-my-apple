using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users;

public class User : IdentityUser
{
    required public string FirstName { get; set; } = default!;
    required public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public Image? Image { get; set; } = null;
    required public DateTime BirthDay { get; set; }
    required public string Country { get; set; }

    public static User Create(Guid guid,
                              string email,
                              string passwordHash,
                              string firstName,
                              string lastName,
                              string phoneNumber,
                              DateTime birthDay,
                              Image? image,
                              string country)
    {
        return new User
        {
            Id = guid.ToString(),
            Email = email,
            UserName = email,
            NormalizedEmail = email.ToUpper(),
            NormalizedUserName = email.ToUpper(),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = false,
            BirthDay = birthDay,
            Image = image,
            Country = country
        };
    }
}
