

using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderItem : Entity<OrderItemId>, IAuditable, ISoftDelete
{
    private OrderItem(OrderItemId id) : base(id) { }

    public TenantId? TenantId { get; set; }
    public BranchId? BranchId { get; set; }
    public required OrderId OrderId { get; init; }
    public string? SkuId { get; init; }
    public required string ModelId { get; init; }
    public required string ModelName { get; init; }
    public required string ColorName { get; init; }
    public required string StorageName { get; init; }
    public required decimal UnitPrice { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required string ModelSlug { get; init; }
    public required int Quantity { get; init; }
    public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public decimal? DiscountAmount { get; init; }
    public decimal SubTotalAmount
    {
        get
        {
            if (DiscountType == EDiscountType.PERCENTAGE.Name)
            {
                return UnitPrice - (UnitPrice * (decimal)(DiscountValue ?? 0));
            }
            else
            {
                return UnitPrice - (decimal)(DiscountValue ?? 0);
            }
        }
        set { }
    }
    public bool IsReviewed { get; private set; } = false;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string? UpdatedBy { get; init; } = null;
    public bool IsDeleted { get; init; } = false;
    public DateTime? DeletedAt { get; init; } = null;
    public string? DeletedBy { get; init; } = null;

    public static OrderItem Create(OrderItemId orderItemId,
                                   TenantId? tenantId,
                                   BranchId? branchId,
                                   OrderId orderId,
                                   string? skuId,
                                   string modelId,
                                   string modelName,
                                   string colorName,
                                   string storageName,
                                   decimal unitPrice,
                                   string displayImageUrl,
                                   string modelSlug,
                                   int quantity,
                                   string? promotionId,
                                   string? promotionType,
                                   string? discountType,
                                   decimal? discountValue,
                                   decimal? discountAmount,
                                   bool isReviewed)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);

        return new OrderItem(orderItemId)
        {
            TenantId = tenantId,
            BranchId = branchId,
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
            PromotionId = promotionId,
            PromotionType = promotionType,
            DiscountType = discountType,
            DiscountValue = discountValue,
            DiscountAmount = discountAmount,
            IsReviewed = isReviewed
        };
    }

    public void CheckIsReviewed()
    {
        this.IsReviewed = true;
    }
}
