
namespace YGZ.Identity.Contracts.Identity;

public sealed record CreateUserRequest(string FirstName, string LastName, string Email, string Password) { }

