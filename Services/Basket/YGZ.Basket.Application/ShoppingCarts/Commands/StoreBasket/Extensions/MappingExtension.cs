

using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket.Extensions;

public static class MappingExtension
{
    public static ShoppingCart ToEntity(this StoreBasketCommand dto, string userEmail)
    {
        var cartItems = dto.Cart.Select(x => x.ToEntity()).ToList();

        return ShoppingCart.Create(userEmail, cartItems);
    }

    public static ShoppingCartItem ToEntity(this CartItemCommand dto)
    {
        return ShoppingCartItem.Create(ProductId: dto.ProductId,
                                       ProductModel: dto.ProductModel,
                                       ProductColor: dto.ProductColor,
                                       ProductColorHex: dto.ProductColorHex,
                                       ProductStorage: dto.ProductStorage,
                                       ProductPrice: dto.ProductPrice,
                                       ProductImage: dto.ProductImage,
                                       Quantity: dto.Quantity);
    }
}