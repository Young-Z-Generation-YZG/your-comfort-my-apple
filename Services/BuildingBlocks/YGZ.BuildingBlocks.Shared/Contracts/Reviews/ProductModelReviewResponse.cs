
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Reviews;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ProductModelReviewResponse
{
    public required string Id { get; init; }
    public required string ModelId { get; init; }
    public required string SkuId { get; init; }
    public required OrderInfoResponse OrderInfo { get; init; }
    public required CustomerReviewInfoResponse CustomerReviewInfo { get; init; }
    public required int Rating { get; init; }
    public required string Content { get; init; }
    public required string CreatedAt { get; init; }
    public required string UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public required bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CustomerReviewInfoResponse {
    public required string Name { get; init; }
    public string? AvatarImageUrl { get; init; }
    public string? UserId { get; init; }
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record OrderInfoResponse {
    public required string OrderId { get; init; }
    public required string OrderItemId { get; init; }
}