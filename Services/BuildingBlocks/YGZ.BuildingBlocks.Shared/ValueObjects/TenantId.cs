using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.BuildingBlocks.Shared.ValueObjects;

public class TenantId : ValueObject
{
    public string? Value { get; private set; }


    private TenantId(string? id)
    {
        Value = id;
    }

    public static TenantId Create(string id)
    {
        return new TenantId(id);
    }

    public static TenantId Of(string? id)
    {
        return new TenantId(id ?? null);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value ?? string.Empty;
    }
}

