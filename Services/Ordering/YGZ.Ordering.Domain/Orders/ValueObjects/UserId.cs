

using YGZ.Ordering.Domain.Core.Exceptions;
using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }

    private UserId(Guid guid) => Value = guid;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static UserId Create()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Of(Guid guid)
    {
        ArgumentNullException.ThrowIfNull(guid);

        if (guid == Guid.Empty)
        {
            throw new DomainException("UserId cannot be empty.");
        }


        return new UserId(guid);
    }
}
