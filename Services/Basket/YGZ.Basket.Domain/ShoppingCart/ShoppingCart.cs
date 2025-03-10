

using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Domain.ShoppingCart;

public class ShoppingCart
{
    public string UserEmail { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();


    public static ShoppingCart Create(string userEmail, List<ShoppingCartItem> cartItems)
    {
        return new ShoppingCart
        {
            UserEmail = userEmail,
            Items = cartItems
        };
    }
}