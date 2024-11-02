

using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Entities;

public class Inventory : Entity<InventoryId>
{
    public int QuantityInStock { get; }
    public Inventory(InventoryId inventoryId, int quantityInStock) : base(inventoryId)
    {
        QuantityInStock = quantityInStock;
    }

    public static Inventory Create(int quantityInStock)
    {
        return new Inventory(InventoryId.CreateUnique(), quantityInStock);
    }
}
