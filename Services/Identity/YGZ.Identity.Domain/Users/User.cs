
using Microsoft.AspNetCore.Identity;

namespace YGZ.Identity.Domain.Users;

public class User : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
}
