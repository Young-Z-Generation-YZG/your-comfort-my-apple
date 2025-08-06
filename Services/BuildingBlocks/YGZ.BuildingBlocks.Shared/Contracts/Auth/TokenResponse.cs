
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record TokenResponse
{
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public int RefreshExpiresIn { get; init; }
    public string RefreshToken { get; init; }
    public int NotBeforePolicy { get; init; }
    public string TokenType { get; init; }
    public string Scope { get; init; }
}