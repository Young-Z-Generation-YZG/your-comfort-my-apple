namespace YGZ.Keycloak.Api.Contracts;

public sealed record LoginRequest(string Email, string Password) { }