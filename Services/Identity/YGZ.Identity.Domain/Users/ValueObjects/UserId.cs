

using YGZ.Identity.Domain.Core.Primitives;

namespace YGZ.Identity.Domain.Users.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }

    public UserId( Guid value)
    {
        Value = value;
    }

    public static UserId CreateNew(Guid value)
    {
        return new UserId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
