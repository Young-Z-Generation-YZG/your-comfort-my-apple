

using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Domain.ShoppingCart;

public class ShoppingCart
{
    required public string UserEmail { get; set; }
    public List<ShoppingCartItem> CartItems { get; set; } = new();
    public decimal SubTotalAmount => CartItems.Where(item => item.IsSelected).Sum(item => item.SubTotalAmount);
    public string? PromotionId { get; set; }
    public string? PromotionType { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public string? DiscountCouponError { get; set; }
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
        CartItems = CartItems.Where(ci => ci.Promotion?.PromotionEvent == null).ToList();

        return this;
    }

    public ShoppingCart FilterOutEventItemsAndNotSelectedItem()
    {
        CartItems = CartItems.Where(ci => ci.Promotion?.PromotionEvent == null).Where(ci => ci.IsSelected == true).ToList();

        return this;
    }

    public ShoppingCart FilterSelectedItem()
    {
        CartItems = CartItems.Where(ci => ci.IsSelected == true).ToList();

        return this;
    }

    public ShoppingCart FilterEventItems()
    {
        CartItems = CartItems.Where(ci => ci.Promotion?.PromotionEvent != null).Where(ci => ci.IsSelected == true).ToList();

        return this;
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

    public void AddCartItem(ShoppingCartItem cartItem)
    {
        // check current cart item exists
        var existingItem = CartItems.FirstOrDefault(ci => ci.SkuId == cartItem.SkuId);

        // increase quantity if exists
        // otherwise, add new item
        // SIDE EFFECT: set highest order for this item
        if (existingItem != null)
        {
            // update quantity
            existingItem.Order = CartItems.Count + 1;
            existingItem.Quantity += cartItem.Quantity;
        }
        else
        {
            // add new item
            cartItem.Order = CartItems.Count + 1;
            CartItems.Add(cartItem);
        }

    }

    public void SetDiscountCouponError(string message)
    {
        DiscountCouponError = message;
    }

    public GetBasketResponse ToResponse()
    {

        return new GetBasketResponse()
        {
            UserEmail = this.UserEmail,
            CartItems = this.CartItems.Select(item => item.ToResponse()).ToList(),
            SubTotalAmount = this.SubTotalAmount,
            PromotionId = this.PromotionId,
            PromotionType = this.PromotionType,
            DiscountType = this.DiscountType,
            DiscountValue = this.DiscountValue,
            DiscountAmount = this.DiscountAmount,
            MaxDiscountAmount = this.MaxDiscountAmount,
            DiscountCouponError = this.DiscountCouponError,
            TotalAmount = this.TotalAmount
        };
    }
}