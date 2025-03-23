

using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;

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
        return ShoppingCartItem.Create(productId: dto.ProductId,
                                       productModel: dto.ProductModel,
                                       productColor: dto.ProductColor,
                                       productStorage: dto.ProductStorage,
                                       productUnitPrice: dto.ProductUnitPrice,
                                       productNameTag: dto.ProductNameTag,
                                       productImage: dto.ProductImage,
                                       quantity: dto.Quantity);
    }
}