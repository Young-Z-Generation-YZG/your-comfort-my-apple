

using YGZ.Basket.Domain.Core.Primitives;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class CartLine : Entity<CartLineId>
{
    public string ProductId { get; private set; }

    public string Sku { get; private set; }
    public string Model { get; private set; }
    public string Color { get; private set; }
    public string Storage { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public string Image_url { get; private set; }

    public decimal SubTotal => Price * Quantity;

    public CartLine(CartLineId id, string productId, string sku, string model, string color, string storage, int quantity, decimal price, string imageUrl) : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        Sku = sku;
        Model = model;
        Color = color;
        Storage = storage;
        Image_url = imageUrl;
    }

    public static CartLine CreateNew(string productId, string sku, string model, string color, string storage, int quantity, decimal price, string imageUrl)
    {
        return new(CartLineId.CreateUnique(), productId, sku, model, color, storage, quantity, price, imageUrl);
    }
}
