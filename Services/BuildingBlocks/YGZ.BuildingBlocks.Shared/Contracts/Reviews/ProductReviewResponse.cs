
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Reviews;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record ProductReviewResponse
{
    required public string ReviewId { get; set; }
    required public string CustomerUsername {  get; set; }
    required public string CustomerImage {  get; set; }
    required public int Rating { get; set; }
    required public string Content { get; set; }
    required public string CreatedAt { get; set; }
}
