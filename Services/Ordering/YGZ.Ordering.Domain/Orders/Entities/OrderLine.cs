
using YGZ.Ordering.Domain.Core.Abstractions;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderLine : Entity<OrderLineId>, IAuditable
{
    public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private OrderLine(OrderLineId id) : base(id)
    {

    }
}
