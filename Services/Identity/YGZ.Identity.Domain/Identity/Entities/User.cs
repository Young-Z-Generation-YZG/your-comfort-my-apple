using Microsoft.AspNetCore.Identity;

namespace YGZ.Identity.Domain.Identity.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string FullName
    {
        get => $"{FirstName} {LastName}";
    }
}
