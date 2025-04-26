
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record ResetPasswordVerificationResponse
{
    required public Dictionary<string, string> Params { get; set; }
    required public string VerificationType { get; set; }
}
