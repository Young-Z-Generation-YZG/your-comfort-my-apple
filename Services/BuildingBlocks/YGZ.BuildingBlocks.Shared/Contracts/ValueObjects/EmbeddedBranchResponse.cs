using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record EmbeddedBranchResponse
{
    public required string BranchId { get; init; }
    public required string BranchName { get; init; }
}
