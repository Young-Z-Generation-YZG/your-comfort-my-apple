using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class ETenantType : SmartEnum<ETenantType>
{
    public ETenantType(string name, int value) : base(name, value) { }

    public static readonly ETenantType UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly ETenantType WAREHOUSE = new(nameof(WAREHOUSE), 0);
    public static readonly ETenantType BRANCH = new(nameof(BRANCH), 0);
}
