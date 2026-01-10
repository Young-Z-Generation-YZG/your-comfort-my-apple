using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ReservedForSkuRequestResponse
{
    public required string ToBranchId { get; init; }
    public required string ToBranchName { get; init; }
    public required int ReservedQuantity { get; init; }
}
