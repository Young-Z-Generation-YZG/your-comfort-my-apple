
using YGZ.Ordering.Domain.Core.Exceptions;
using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderId : ValueObject
{
    public Guid Value { get; private set; }

    private OrderId(Guid guid) => Value = guid;


    public static OrderId Create()
    {
        return new OrderId(Guid.NewGuid());
    }

    public static OrderId Of(Guid guid)
    {
        ArgumentNullException.ThrowIfNull(guid);

        if(guid == Guid.Empty)
        {
            throw new DomainException("OrderId cannot be empty.");
        }

        return new OrderId(guid);
    }

    public static OrderId Of(string id)
    {
        Guid.TryParse(id, out var guid);

        return new OrderId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
