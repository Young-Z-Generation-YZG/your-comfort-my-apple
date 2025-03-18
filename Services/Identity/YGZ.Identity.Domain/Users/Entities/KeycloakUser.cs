

namespace YGZ.Identity.Domain.Users.Entities;

public class KeycloakUser
{
    public string Id { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public bool EmailVerified { get; set; } = default!;
}
