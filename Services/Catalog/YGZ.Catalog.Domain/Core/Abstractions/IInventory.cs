

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IInventory
{
    public int QuantityInStock { get; set; }
    public int QuantityRemain { get; set; }
    public int Sold { get; set; }
}
