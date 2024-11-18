
using YGZ.Basket.Domain.Core.Abstractions;
using YGZ.Basket.Domain.Core.Primitives;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart;

public class ShoppingCart : AggregateRoot<ShoppingCartId>, IAuditable
{
    public UserId UserId { get; private set; }

    public List<CartLine> CartLines { get; set; } = new();

    public decimal TotalPrice => CartLines.Sum(line => line.SubTotal);

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }


    public ShoppingCart(ShoppingCartId id, UserId userId, List<CartLine> cartLines, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        UserId = userId;
        CartLines = cartLines;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ShoppingCart CreateNew(UserId userId)
    {
        return new(ShoppingCartId.CreateUnique(), userId, new List<CartLine>(), DateTime.UtcNow, DateTime.UtcNow);
    }
}
