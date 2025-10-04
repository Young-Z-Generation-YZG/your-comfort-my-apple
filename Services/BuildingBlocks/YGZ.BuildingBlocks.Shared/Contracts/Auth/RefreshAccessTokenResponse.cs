
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record RefreshAccessTokenResponse
{
    public required string AccessToken { get; init; }

    public required int AccessTokenExpiresInSeconds { get; init; }
}
