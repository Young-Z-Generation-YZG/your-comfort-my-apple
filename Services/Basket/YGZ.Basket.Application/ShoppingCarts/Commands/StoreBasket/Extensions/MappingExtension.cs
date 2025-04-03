

using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket.Extensions;

public static class MappingExtension
{
    public static ShoppingCart ToEntity(this StoreBasketCommand dto, string userEmail)
    {
        var cartItems = dto.CartItems.Select(x => x.ToEntity()).ToList();

        return ShoppingCart.Create(userEmail, cartItems);
    }

    public static ShoppingCartItem ToEntity(this CartItemCommand dto)
    {
        Promotion? promotion = null;

        if (dto.Promotion is not null)
        {
            promotion = Promotion.Create(promotionIdOrCode: dto.Promotion.PromotionIdOrCode,
                                         promotionEventType: dto.Promotion.PromotionEventType,
                                         promotionTitle: null!,
                                         promotionDiscountType: null,
                                         promotionDiscountValue: null,
                                         promotionDiscountUnitPrice: null,
                                         promotionAppliedProductCount: null,
                                         promotionFinalPrice: null);
        }

        return ShoppingCartItem.Create(productId: dto.ProductId,
                                       productName: dto.ProductName,
                                       productColorName: dto.ProductColorName,
                                       productUnitPrice: dto.ProductUnitPrice,
                                       productNameTag: dto.ProductNameTag,
                                       productSlug: dto.ProductSlug,
                                       categoryId: dto.CategoryId,
                                       productImage: dto.ProductImage,
                                       quantity: dto.Quantity,
                                       promotion: promotion,
                                       subTotalAmount: null,
                                       orderIndex: dto.OrderIndex);
    }
}