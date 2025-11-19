using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;


public sealed record CreateOrderCommand : ICommand<bool>
{
    public required Guid OrderId { get; init; }
    public required string TenantId { get; init; }
    public required string BranchId { get; init; }
    public required string CustomerId { get; init; }
    public string? CustomerPublicKey { get; init; }
    public string? Tx { get; init; }
    public required string CustomerEmail { get; init; }
    public required ShippingAddressCommand ShippingAddress { get; init; }
    public PromotionInOrderCommand? Promotion { get; init; }
    public required List<OrderItemCommand> OrderItems { get; init; }
    public required string PaymentMethod { get; init; }
    public required decimal TotalAmount { get; init; }

}

public sealed record ShippingAddressCommand
{
    public required string ContactName { get; init; }
    public required string ContactPhoneNumber { get; init; }
    public required string AddressLine { get; init; }
    public required string District { get; init; }
    public required string Province { get; init; }
    public required string Country { get; init; }
}

public sealed record OrderItemCommand
{
    public required string ModelId { get; init; }
    public required string SkuId { get; init; }
    public required string ProductName { get; init; }
    public required string NormalizedModel { get; init; }
    public required string NormalizedColor { get; init; }
    public required string NormalizedStorage { get; init; }
    public required decimal UnitPrice { get; init; }
    public required string DisplayImageUrl { get; init; }
    public PromotionInItemCommand? Promotion { get; init; }
    public required int Quantity { get; init; }
    public required decimal SubTotalAmount { get; init; }
    public required string ModelSlug { get; init; }
}

public sealed record PromotionInItemCommand
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
}

public sealed record PromotionInOrderCommand
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
}