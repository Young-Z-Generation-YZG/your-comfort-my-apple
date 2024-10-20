namespace YGZ.Identity.Contracts.Identity.CreateUser;

public sealed record CreateUserRequest(string FirstName, string LastName, string Email, string Password) { }

