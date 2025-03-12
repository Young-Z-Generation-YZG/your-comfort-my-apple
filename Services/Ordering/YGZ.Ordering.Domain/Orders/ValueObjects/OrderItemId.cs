

using YGZ.Ordering.Domain.Core.Exceptions;
using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderItemId : ValueObject
{
    public Guid Value { get; }
    private OrderItemId(Guid guid) => Value = guid;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static OrderItemId Create()
    {
        return new OrderItemId(Guid.NewGuid());
    }

    public static OrderItemId Of(Guid guid)
    {
        ArgumentNullException.ThrowIfNull(guid);

        if (guid == Guid.Empty)
        {
            throw new DomainException("OrderItemId cannot be empty.");
        }


        return new OrderItemId(guid);
    }
}
