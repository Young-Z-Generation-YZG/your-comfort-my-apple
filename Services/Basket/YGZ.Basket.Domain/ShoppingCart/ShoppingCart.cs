

using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Domain.ShoppingCart;

public class ShoppingCart
{
    required public string UserEmail { get; set; }
    public List<ShoppingCartItem> CartItems { get; set; } = new();
    public decimal TotalAmount => CartItems.Where(x => x.IsSelected).Sum(x => x.SubTotalAmount);

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

    //public void ApplyCoupon(CouponModel coupon)
    //{
    //    foreach (var item in CartItems)
    //    {
    //        if (item.Promotion == null)
    //        {
    //            item.Promotion = new ValueObjects.Promotion
    //            {
    //                Coupon = ValueObjects.Coupon.Create(coupon.Id, coupon.Code, coupon.DiscountType, coupon.DiscountValue)
    //            };
    //        }
    //        else
    //        {
    //            item.Promotion.Coupon = ValueObjects.Coupon.Create(coupon.Id, coupon.Code, coupon.DiscountType, coupon.DiscountValue);
    //        }
    //    }
    //}
}