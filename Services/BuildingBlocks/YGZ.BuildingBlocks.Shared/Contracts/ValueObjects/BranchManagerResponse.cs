using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record BranchManagerResponse
{
    required public string Id { get; init; }
    required public string Name { get; init; }
    required public string Email { get; init; }
    required public string PhoneNumber { get; init; }
}
