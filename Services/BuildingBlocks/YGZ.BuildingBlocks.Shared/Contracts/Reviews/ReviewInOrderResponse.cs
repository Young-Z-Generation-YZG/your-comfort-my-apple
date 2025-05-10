

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Reviews;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record ReviewInOrderResponse
{
    public required string ReviewId { get; set; }
    public required string ProductId { get; set; }
    public required string ModelId { get; set; }
    public required string OrderId { get; set; }
    public required string OrderItemId { get; set; }
    public required int Rating { get; set; }
    public required string Content { get; set; }
    public required string CreatedAt { get; set; }
}
