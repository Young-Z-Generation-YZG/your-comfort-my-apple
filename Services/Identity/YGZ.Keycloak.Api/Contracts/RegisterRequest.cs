namespace YGZ.Keycloak.Api.Contracts;

public sealed record RegisterRequest(string Email, string Password, string Firstname, string LastName) { }