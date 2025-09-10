using System.Text.Json.Serialization;
namespace YGZ.Identity.Domain.Users.Entities;

public sealed record KeycloakUser
{
  [JsonPropertyName("id")]
  public required string Id { get; init; }

  [JsonPropertyName("username")]
  public required string Username { get; init; }

  [JsonPropertyName("firstName")]
  public required string FirstName { get; init; }

  [JsonPropertyName("lastName")]
  public required string LastName { get; init; }

  [JsonPropertyName("email")]
  public required string Email { get; init; }

  [JsonPropertyName("emailVerified")]
  public required bool EmailVerified { get; init; }
}