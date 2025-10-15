using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.BuildingBlocks.Shared.ValueObjects;

public class TenantId : ValueObject
{
    public Guid Value { get; private set; }


    private TenantId(Guid guid)
    {
        Value = guid;
    }

    public static TenantId Create()
    {
        return new TenantId(Guid.NewGuid());
    }

    public static TenantId Of(string guid)
    {
        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new TenantId(parsedGuid);
    }

    public static TenantId Of(Guid guid)
    {
        return new TenantId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

