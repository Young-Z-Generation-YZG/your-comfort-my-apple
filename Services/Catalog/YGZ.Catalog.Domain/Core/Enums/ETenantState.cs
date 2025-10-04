using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class ETenantState : SmartEnum<ETenantState>
{
    public ETenantState(string name, int value) : base(name, value) { }

    public static readonly ETenantState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly ETenantState ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly ETenantState INACTIVE = new(nameof(INACTIVE), 0);
    public static readonly ETenantState MAINTANCE = new(nameof(MAINTANCE), 0);
}
