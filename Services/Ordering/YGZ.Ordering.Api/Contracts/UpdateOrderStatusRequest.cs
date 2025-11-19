using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;


namespace YGZ.Ordering.Api.Contracts;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record UpdateOrderStatusRequest()
{
    [Required]
    public required string UpdateStatus { get; init; }
}
