

using YGZ.Keycloak.Domain.Core.Primitives;

namespace YGZ.Keycloak.Domain.Users.ValueObjects;

public class UserId : ValueObject, IEquatable<UserId>
{
    public Guid Value { get; private set; }

    public static UserId Create(Guid value)
    {
        return new UserId { Value = value };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public bool Equals(UserId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as UserId);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
