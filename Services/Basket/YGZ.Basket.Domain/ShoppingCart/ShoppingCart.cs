
using YGZ.Basket.Domain.Core.Abstractions;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.Core.Primitives;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.ShoppingCart;

public class ShoppingCart : AggregateRoot<ShoppingCartId>, IAuditable
{
    public string UserIdValue => UserId.Value.ToString();
    public UserId UserId { get; private set; }

    public List<CartLine> CartLines { get; set; } = new();

    public double Total => CartLines.Sum(line => line.SubTotal);

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }


    public ShoppingCart(ShoppingCartId id, UserId userId, List<CartLine> cartLines, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        UserId = userId;
        CartLines = cartLines;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ShoppingCart CreateNew(Guid userId, List<CartLine> cartLines)
    {
        return new(ShoppingCartId.CreateUnique(), UserId.ToEntity(userId), cartLines, DateTime.UtcNow, DateTime.UtcNow);
    }
}
