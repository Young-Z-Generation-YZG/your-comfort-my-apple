
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ResetPasswordVerificationResponse
{
    public required Dictionary<string, string> Params { get; set; }
    public required string VerificationType { get; set; }
}
