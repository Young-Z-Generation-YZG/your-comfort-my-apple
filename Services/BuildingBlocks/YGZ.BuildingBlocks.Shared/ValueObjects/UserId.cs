using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.BuildingBlocks.Shared.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }

    private UserId(Guid guid)
    {
        Value = guid;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static UserId Create()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Of(string guid)
    {
        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new UserId(parsedGuid);
    }
}
