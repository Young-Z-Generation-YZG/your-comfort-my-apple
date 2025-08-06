
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record LoginResponse() 
{
    public required string UserEmail { get; init;}
    public required string Username { get; init; }
    public string? AccessToken { get; init; } 
    public string? RefreshToken { get; init; }
    public double? AccessTokenExpiresInSeconds { get; init; }
    public double? RefreshTokenExpiresInSeconds { get; init; }
    public required string VerificationType { get; init; }
    public Dictionary<string, string>? Params { get; init; }
}