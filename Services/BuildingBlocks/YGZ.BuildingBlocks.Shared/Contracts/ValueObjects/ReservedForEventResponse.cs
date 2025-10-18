using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ReservedForEventResponse
{
    public required string EventId { get; init; }
    public required string EventItemId { get; init; }
    public required string EventName { get; init; }
    public required int ReservedQuantity { get; init; }
}
