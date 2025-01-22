

namespace YGZ.BuildingBlocks.Shared.Contracts.Identity;

public sealed record LoginResponse(string AccessToken, string RefreshToken, string Expiration) { }