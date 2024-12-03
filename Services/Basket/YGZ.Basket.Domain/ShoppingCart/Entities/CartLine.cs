

using YGZ.Basket.Domain.Core.Primitives;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class CartLine : Entity<CartLineId>
{
    public string ProductItemId { get; private set; }
    public string Model { get; private set; }
    public string Color { get; private set; }
    public int Storage { get; private set; }
    public string PrimaryImageUrl { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal DiscountAmount { get; private set; } = 0;
    public decimal SubTotal => Quantity * (Price - DiscountAmount);

    public CartLine(
        CartLineId id,
        string productItemId,
        string model,
        string color,
        int storage,
        string primaryImageUrl,
        int quantity,
        decimal price,
        decimal discountPrice) : base(id)
    {
        ProductItemId = productItemId;
        Model = model;
        Color = color;
        Storage = storage;
        PrimaryImageUrl = primaryImageUrl;
        Price = price;
        Quantity = quantity;
        DiscountAmount = discountPrice;
    }

    public static CartLine CreateNew(string productItemId,
                                     string model,
                                     string color,
                                     int storage,
                                     string primaryImageUrl,
                                     int quantity,
                                     decimal price,
                                     decimal discountAmount)
    {
        return new(CartLineId.CreateUnique(),
                   productItemId,
                   model,
                   color,
                   storage,
                   primaryImageUrl,
                   quantity,
                   price,
                   discountAmount);
    }
}
