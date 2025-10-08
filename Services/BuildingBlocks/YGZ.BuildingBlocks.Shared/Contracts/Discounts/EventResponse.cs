using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record EventResponse
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
