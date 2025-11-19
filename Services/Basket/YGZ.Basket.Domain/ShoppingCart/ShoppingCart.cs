

using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Domain.ShoppingCart;

public class ShoppingCart
{
    required public string UserEmail { get; set; }
    public List<ShoppingCartItem> CartItems { get; set; } = new();
    public decimal SubTotalAmount => CartItems.Sum(item => item.SubTotalAmount);
    public string? PromotionId { get; set; }
    public string? PromotionType { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public decimal TotalAmount => SubTotalAmount - (DiscountAmount ?? 0);

    public static ShoppingCart Create(string userEmail, List<ShoppingCartItem> cartItems)
    {
        return new ShoppingCart
        {
            UserEmail = userEmail,
            CartItems = cartItems,
        };
    }

    public ShoppingCart FilterOutEventItems()
    {
        return new ShoppingCart
        {
            UserEmail = UserEmail,
            CartItems = CartItems.Where(ci => ci.Promotion?.PromotionEvent == null).ToList(),
        };
    }

    public ShoppingCart FilterOutEventItemsAndSelected()
    {
        return new ShoppingCart
        {
            UserEmail = UserEmail,
            CartItems = CartItems.Where(ci => ci.Promotion?.PromotionEvent == null).Where(ci => ci.IsSelected == true).ToList(),
        };
    }

    public ShoppingCart FilterEventItems()
    {
        return new ShoppingCart
        {
            UserEmail = UserEmail,
            CartItems = CartItems.Where(ci => ci.Promotion?.PromotionEvent != null).Where(ci => ci.IsSelected == true).ToList(),
        };
    }

    public bool CheckHasEventItems()
    {
        return CartItems.Any(ci => ci.Promotion?.PromotionEvent != null);
    }

    public ShoppingCart RemoveEventItems()
    {
        return new ShoppingCart
        {
            UserEmail = UserEmail,
            CartItems = CartItems.Where(ci => ci.Promotion?.PromotionEvent == null).ToList(),
        };
    }
}