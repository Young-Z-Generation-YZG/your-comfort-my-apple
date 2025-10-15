

using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderItemId : ValueObject
{
    public Guid Value { get; init; }
    private OrderItemId(Guid guid)
    {
        Value = guid;
    }

    public static OrderItemId Create()
    {
        return new OrderItemId(Guid.NewGuid());
    }

    public static OrderItemId Of(string guid)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(guid);

        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new OrderItemId(parsedGuid);
    }

    public static OrderItemId Of(Guid guid)
    {
        return new OrderItemId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
