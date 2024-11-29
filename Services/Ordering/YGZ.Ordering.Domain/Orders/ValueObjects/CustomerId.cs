

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class CustomerId : ValueObject
{
    public Guid Value { get; private set; } = default!;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
