using Microsoft.AspNetCore.Identity;

namespace YGZ.Identity.Domain.Users;

public class User : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";

    public static User Create(string email, string passwordHash, string firstName, string lastName)
    {
        return new User
        {
            Email = email,
            UserName = email,
            NormalizedEmail = email.ToUpper(),
            NormalizedUserName = email.ToUpper(),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName
        };
    }
}
