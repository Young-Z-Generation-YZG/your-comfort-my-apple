

using YGZ.Basket.Domain.Common.ValueObjects;
using YGZ.Basket.Domain.Core.Primitives;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart.Entities;

public class CartLine : Entity<CartLineId>
{
    public ProductId ProductId { get; private set; }

    public int Quantity { get; private set; }

    public Price Price { get; private set; }

    public decimal SubTotal => Price.Amount * Quantity;

    public CartLine(CartLineId id, ProductId productId, int quantity, Price price) : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public static CartLine CreateNew(ProductId productId, int Quantity, Price price)
    {
        return new(CartLineId.CreateUnique(), productId, Quantity, price);
    }
}
