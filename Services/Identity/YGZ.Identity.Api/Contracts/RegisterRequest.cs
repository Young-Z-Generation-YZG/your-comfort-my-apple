namespace YGZ.Identity.Api.Contracts;

public sealed record RegisterRequest(string FirstName, string LastName, string Email, string Password) { }