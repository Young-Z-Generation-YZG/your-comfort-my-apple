using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.BuildingBlocks.Shared.Utils;

public class EfficientCart
{
    public List<EfficientCartItem> CartItems { get; set; } = new();
    public decimal SubTotalAmount => CartItems.Sum(item => item.SubTotalAmount);
    public string? PromotionId { get; set; }
    public string? PromotionType { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public decimal TotalAmount => SubTotalAmount - (DiscountAmount ?? 0);
}

public class EfficientCartItem
{
    public required string UniqueString { get; set; }
    public required decimal OriginalPrice { get; set; }
    public required int Quantity { get; set; }
    public decimal SubTotalAmount => OriginalPrice * Quantity;
    public string? PromotionId { get; set; }
    public string? PromotionType { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal TotalAmount => SubTotalAmount - (DiscountAmount ?? 0);
}

public static class CalculatePrice
{
    /// <summary>
    /// Apply discount to the original price
    /// </summary>
    /// <param name="originalPrice">The original price</param>
    /// <param name="discountType">The discount type</param>
    /// <param name="discountValue">The discount value (from 0 to 100 for percentage)</param>
    /// <param name="quantity">The quantity of the item</param>
    /// <returns>The discounted price</returns>
    public static decimal CalculateDiscountAmount(decimal originalPrice, string discountType, decimal discountValue, int quantity)
    {
        EDiscountType.TryFromName(discountType, true, out var discountTypeEnum);

        // Calculate total price before discount
        decimal totalPrice = originalPrice * quantity;

        if (discountTypeEnum.Equals(EDiscountType.PERCENTAGE))
        {
            // Apply percentage discount to total price
            // Formula: totalPrice * (1 - discountValue / 100)
            decimal discountAmount = totalPrice * (discountValue / 100m);
            return totalPrice - discountAmount;
        }
        else if (discountTypeEnum.Equals(EDiscountType.FIXED_AMOUNT))
        {
            // Apply fixed amount discount per unit to total price
            // discountValue is per unit, so multiply by quantity
            decimal discountAmount = discountValue * quantity;
            return totalPrice - discountAmount;
        }

        throw new ArgumentException("Invalid discount type");
    }

    public static EfficientCart CalculateEfficientCart(EfficientCart beforeCart, string discountType, decimal discountValue, decimal? maxDiscountAmount = null)
    {
        EDiscountType.TryFromName(discountType, true, out var discountTypeEnum);

        if(discountTypeEnum is null)
        {
            throw new ArgumentException("Invalid discount type");
        }

        // Use SubTotalAmount as base for discount calculation (before any previous discounts)
        decimal subtotalAmount = beforeCart.SubTotalAmount;

        var newCart = new EfficientCart
        {
            // Copy promotion information from beforeCart
            PromotionId = beforeCart.PromotionId,
            PromotionType = beforeCart.PromotionType,
            DiscountType = discountType,
            DiscountValue = discountValue,
            MaxDiscountAmount = maxDiscountAmount
        };

        decimal calculatedDiscount = 0;

        if(discountTypeEnum.Equals(EDiscountType.PERCENTAGE))
        {
            calculatedDiscount = subtotalAmount * (discountValue / 100m);
        } else if(discountTypeEnum.Equals(EDiscountType.FIXED_AMOUNT))
        {
            calculatedDiscount = discountValue;
        }

        decimal actualTotalDiscount = maxDiscountAmount.HasValue && maxDiscountAmount > 0
            ? Math.Min(calculatedDiscount, maxDiscountAmount.Value)
            : calculatedDiscount;

        newCart.DiscountAmount = actualTotalDiscount;

        foreach(var item in beforeCart.CartItems)
        {
            decimal itemSubtotal = item.SubTotalAmount;
            decimal itemProportion = subtotalAmount > 0 ? itemSubtotal / subtotalAmount : 0;
            decimal itemTotalDiscount = actualTotalDiscount * itemProportion;

            decimal discountPerUnit = item.Quantity > 0 ? itemTotalDiscount / item.Quantity : 0;

            decimal effectiveDiscountValue = item.OriginalPrice > 0
                ? discountPerUnit / item.OriginalPrice
                : 0;

            var newItem = new EfficientCartItem
            {
                UniqueString = item.UniqueString,
                OriginalPrice = item.OriginalPrice,
                Quantity = item.Quantity,
                // Copy promotion information from original item
                PromotionId = item.PromotionId,
                PromotionType = item.PromotionType,
                DiscountType = discountType,
                DiscountValue = effectiveDiscountValue * 100,
                DiscountAmount = itemTotalDiscount
            };

            newCart.CartItems.Add(newItem);
        }

        return newCart;
    }

    public static decimal CalculateSubTotalAmount(decimal originalPrice, int quantity)
    {
        return originalPrice * quantity;
    }
}
