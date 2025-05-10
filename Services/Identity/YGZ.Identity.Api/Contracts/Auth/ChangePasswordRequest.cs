using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.Identity.Api.Contracts.Auth;

public class ChangePasswordRequest
{
    [JsonPropertyName("old_password")]
    public required string OldPassword { get; init; }

    [JsonPropertyName("new_password")]
    public required string NewPassword { get; init; }
}