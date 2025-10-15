using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderId : ValueObject
{
    public Guid Value { get; init; }

    private OrderId(Guid guid)
    {
        Value = guid;
    }

    public static OrderId Create()
    {
        return new OrderId(Guid.NewGuid());
    }

    public static OrderId Of(string guid)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(guid);

        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new OrderId(parsedGuid);
    }

    public static OrderId Of(Guid guid)
    {
        return new OrderId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
