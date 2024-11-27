

using YGZ.Basket.Domain.Core.Primitives;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class CartLine : Entity<CartLineId>
{
    public string ProductItemId { get; private set; }
    public string Model { get; private set; }
    public string Color { get; private set; }
    public int Storage { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public decimal SubTotal => Price * Quantity;

    public CartLine(
        CartLineId id,
        string productItemId,
        string model,
        string color,
        int storage,
        int quantity,
        decimal price) : base(id)
    {
        ProductItemId = productItemId;
        Quantity = quantity;
        Price = price;
        Model = model;
        Color = color;
        Storage = storage;
    }

    public static CartLine CreateNew(string productItemId, string model, string color, int storage, int quantity, decimal price)
    {
        return new(CartLineId.CreateUnique(), productItemId, model, color, storage, quantity, price);
    }
}
