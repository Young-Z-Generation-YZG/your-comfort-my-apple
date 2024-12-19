

using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class Product : Entity<ProductId>
{
    public string Model { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public int Storage { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;

    public Product(ProductId id, string model, string color, int storage, decimal price) : base(id)
    {
        Model = model;
        Color = color;
        Storage = storage;
        Price = price;
    }

    public static Product CreateNew(ProductId? productId, string model, string color, int storage, decimal price)
    {
        return new Product(productId ?? ProductId.CreateNew(), model, color, storage, price);
    }
}
