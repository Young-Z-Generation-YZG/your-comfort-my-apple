

using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderItem : Entity<OrderItemId>, IAuditable, ISoftDelete
{
    private OrderItem(OrderItemId id) : base(id) { }

    public required OrderId OrderId { get; init; }
    public SkuId? SkuId { get; init; }
    public required string ModelId { get; init; }
    public required string ModelName { get; init; }
    public required string ColorName { get; init; }
    public required string StorageName { get; init; }
    public required decimal UnitPrice { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required string ModelSlug { get; init; }
    public required int Quantity { get; init; }
    public Promotion? Promotion { get; init; } = null;
    public decimal SubTotalAmount { get; init; }
    public bool IsReviewed { get; private set; } = false;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string? UpdatedBy { get; init; } = null;
    public bool IsDeleted { get; init; } = false;
    public DateTime? DeletedAt { get; init; } = null;
    public string? DeletedBy { get; init; } = null;

    public static OrderItem Create(OrderItemId orderItemId,
                                   OrderId orderId,
                                   SkuId? skuId,
                                   string modelId,
                                   string modelName,
                                   string colorName,
                                   string storageName,
                                   decimal unitPrice,
                                   string displayImageUrl,
                                   string modelSlug,
                                   int quantity,
                                   Promotion? promotion,
                                   decimal subTotalAmount,
                                   bool isReviewed)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);

        return new OrderItem(orderItemId)
        {
            OrderId = orderId,
            SkuId = skuId,
            ModelId = modelId,
            ModelName = modelName,
            ColorName = colorName,
            StorageName = storageName,
            UnitPrice = unitPrice,
            DisplayImageUrl = displayImageUrl,
            ModelSlug = modelSlug,
            Quantity = quantity,
            Promotion = promotion,
            SubTotalAmount = subTotalAmount,
            IsReviewed = isReviewed
        };
    }

    public void CheckIsReviewed()
    {
        this.IsReviewed = true;
    }
}
