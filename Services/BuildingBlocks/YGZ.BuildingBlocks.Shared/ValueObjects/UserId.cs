using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.BuildingBlocks.Shared.ValueObjects;

public class UserId : ValueObject, IEquatable<UserId>
{
    public Guid Value { get; private set; }

    public static UserId Create(Guid value)
    {
        return new UserId { Value = value };
    }

    public static UserId Of(string guid)
    {
        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new UserId { Value = parsedGuid };
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
