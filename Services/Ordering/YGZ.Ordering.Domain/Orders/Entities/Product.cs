

using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class Product : Entity<ProductId>
{
    public string Name { get; private set; } = default!;
    public string Model { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public string Storage { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;

    public Product(ProductId id, string model, string color, string storage, decimal price, int quantity) : base(id)
    {
        Model = model;
        Color = color;
        Storage = storage;
        Price = price;
        Quantity = quantity;
    }

    public static Product CreateNew(string model, string color, string storage, decimal price, int quantity)
    {
        return new Product(ProductId.CreateNew(), model, color, storage, price, quantity);
    }
}
