using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.BuildingBlocks.Shared.ValueObjects;

public class BranchId : ValueObject
{
    public string? Value { get; private set; }


    private BranchId(string? id)
    {
        Value = id;
    }

    public static BranchId Create(string id)
    {
        return new BranchId(id);
    }

    public static BranchId Of(string? id)
    {
        return new BranchId(id ?? null);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value ?? string.Empty;
    }
}

