
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Entities;

public class ProductItem : Entity<ProductItemId>
{
    private readonly Inventory inventory = Inventory.Create(0);

    public string Name { get; }
    public string Description { get;}
    public decimal Price { get; }

    public Inventory Inventory => inventory;

    private ProductItem(ProductItemId productItemId, string name, string description, decimal price) : base(productItemId)
    {
        Name = name;
        Description = description;
        Price = price;
    }

    public static ProductItem Create(string name, string description, decimal price)
    {
        return new ProductItem(ProductItemId.CreateUnique(), name, description, price);
    }
}
