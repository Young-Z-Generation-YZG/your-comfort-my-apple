﻿

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderId : ValueObject
{
    public Guid Value { get; private set; }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}