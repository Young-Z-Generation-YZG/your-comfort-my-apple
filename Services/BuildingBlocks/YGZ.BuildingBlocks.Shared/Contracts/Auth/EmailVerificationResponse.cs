
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record EmailVerificationResponse()
{
    required public Dictionary<string, string> Params { get; set; }
    required public string VerificationType { get; set; }
    required public double TokenExpiredIn { get; set; }
}
