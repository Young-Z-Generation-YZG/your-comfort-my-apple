using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.BuildingBlocks.Shared.ValueObjects;

public class BranchId : ValueObject
{
    public Guid Value { get; private set; }

    private BranchId(Guid guid)
    {
        Value = guid;
    }
    public static BranchId Create()
    {
        return new BranchId(Guid.NewGuid());
    }

    public static BranchId Of(string guid)
    {
        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new BranchId(parsedGuid);
    }

    public static BranchId Of(Guid guid)
    {
        return new BranchId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
