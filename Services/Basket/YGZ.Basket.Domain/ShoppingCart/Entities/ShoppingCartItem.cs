

using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class ShoppingCartItem
{
    public required bool IsSelected { get; init; }
    public required string ModelId { get; init; }
    public required string SkuId { get; init; }
    public required string ProductName { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required Model Model { get; init; }
    public required Color Color { get; init; }
    public required Storage Storage { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
    public decimal SubTotalAmount => UnitPrice * Quantity;
    public Promotion? Promotion { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal TotalAmount => SubTotalAmount - (DiscountAmount ?? 0);
    public required string ModelSlug { get; init; }
    public required int Order { get; init; }

    public static ShoppingCartItem Create(
        bool isSelected,
        string modelId,
        string skuId,
        string productName,
        Model model,
        Color color,
        Storage storage,
        string displayImageUrl,
        decimal unitPrice,
        Promotion? promotion,
        int quantity,
        string modelSlug,
        int order
    )
    {
        return new ShoppingCartItem
        {
            IsSelected = isSelected,
            ModelId = modelId,
            SkuId = skuId,
            ProductName = productName,
            Model = model,
            Color = color,
            Storage = storage,
            DisplayImageUrl = displayImageUrl,
            UnitPrice = unitPrice,
            Promotion = promotion,
            Quantity = quantity,
            ModelSlug = modelSlug,
            Order = order
        };
    }

    public CartItemResponse ToResponse()
    {
        return new CartItemResponse
        {
            IsSelected = IsSelected,
            ModelId = ModelId,
            SkuId = SkuId,
            ProductName = ProductName,
            Color = Color.ToResponse(),
            Model = Model.ToResponse(),
            Storage = Storage.ToResponse(),
            DisplayImageUrl = DisplayImageUrl,
            UnitPrice = UnitPrice,
            Quantity = Quantity,
            SubTotalAmount = SubTotalAmount,
            Promotion = Promotion?.ToResponse(),
            DiscountAmount = DiscountAmount,
            TotalAmount = TotalAmount,
            Index = Order
        };
    }

    public CheckoutItemIntegrationEvent ToCheckoutItemIntegrationEvent()
    {
        return new CheckoutItemIntegrationEvent
        {
            ModelId = ModelId,
            SkuId = SkuId,
            ProductName = ProductName,
            NormalizedModel = Model.NormalizedName,
            NormalizedColor = Color.NormalizedName,
            NormalizedStorage = Storage.NormalizedName,
            UnitPrice = UnitPrice,
            DisplayImageUrl = DisplayImageUrl,
            ModelSlug = ModelSlug,
            Quantity = Quantity,
            SubTotalAmount = SubTotalAmount,
            Promotion = Promotion?.ToPromotionIntegrationEvent(),
        };
    }

    public void ApplyCoupon(PromotionCoupon coupon)
    {

        Promotion newPromotion = Promotion.Create(promotionType: EPromotionType.COUPON.Name, PromotionCoupon: coupon, PromotionEvent: null);

        this.Promotion = newPromotion;
    }


}
