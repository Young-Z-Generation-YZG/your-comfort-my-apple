

using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Domain.ShoppingCart;

public class ShoppingCart
{
    required public string UserEmail { get; set; }
    public List<ShoppingCartItem> CartItems { get; set; } = new();
    public decimal TotalAmount => CartItems.Sum(x => x.Quantity * x.SubTotalAmount ?? 0);

    public static ShoppingCart Create(string userEmail, List<ShoppingCartItem> cartItems)
    {
        return new ShoppingCart
        {
            UserEmail = userEmail,
            CartItems = cartItems,
        };
    }
}