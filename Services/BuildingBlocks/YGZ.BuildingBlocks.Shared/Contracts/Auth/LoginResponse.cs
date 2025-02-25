using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record LoginResponse(string AccessToken, string RefreshToken, string Expiration) { }