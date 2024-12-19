
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
    public decimal TotalCostOfGoods { get; set; }
    public decimal TotalDiscountAmount => CartLines.Sum(line => line.DiscountAmount);
    public decimal TotalAmount => TotalCostOfGoods - TotalDiscountAmount;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    public ShoppingCart(ShoppingCartId id, UserId userId, List<CartLine> cartLines, decimal totalCostOfGoods, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        UserId = userId;
        CartLines = cartLines;
        TotalCostOfGoods = totalCostOfGoods;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ShoppingCart CreateNew(Guid userId, List<CartLine> cartLines, decimal TotalCostOfGoods)
    {
        return new(ShoppingCartId.CreateUnique(), UserId.ToEntity(userId), cartLines, TotalCostOfGoods, DateTime.UtcNow, DateTime.UtcNow);
    }
}
