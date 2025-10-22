
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record OrderDetailsResponse()
{
    public string? TenantId { get; init; }
    public string? BranchId { get; init; }
    public required string OrderId { get; init; }
    public required string CustomerId { get; init; }
    public required string CustomerEmail { get; init; }
    public required string OrderCode { get; init; }
    public required string Status { get; init; }
    public required string PaymentMethod { get; init; }
    public required ShippingAddressResponse ShippingAddress { get; init; }
    public required List<OrderItemResponse> OrderItems { get; init; } = new List<OrderItemResponse>();
    public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public decimal? DiscountAmount { get; init; }
    public required decimal TotalAmount { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required string? UpdatedBy { get; init; }
    public required bool IsDeleted { get; init; }
    public required DateTime? DeletedAt { get; init; }
    public required string? DeletedBy { get; init; }
}