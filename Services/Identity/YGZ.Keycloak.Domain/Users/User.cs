


using Microsoft.AspNetCore.Identity;

namespace YGZ.Keycloak.Domain.Users;

public class User : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
}
