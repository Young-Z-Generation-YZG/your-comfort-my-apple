using System.Text.Json.Serialization;

namespace YGZ.Keycloak.Api.Contracts;

public sealed record LoginRequest(
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("password")] string Password)
{ }