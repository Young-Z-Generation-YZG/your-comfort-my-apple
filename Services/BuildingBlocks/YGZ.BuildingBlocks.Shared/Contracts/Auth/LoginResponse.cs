using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed class LoginResponse() 
{
    public required string UserEmail { get; set;}
    public required string Username { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public double? AccessTokenExpiresInSeconds { get; set; }
    public double? RefreshTokenExpiresInSeconds { get; set; }
    public required string VerificationType { get; set; }
    public Dictionary<string, string>? Params { get; set; }
}