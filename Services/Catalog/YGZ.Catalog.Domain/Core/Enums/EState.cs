
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EState : SmartEnum<EState>
{
    public EState(string name, int value) : base(name, value) { }

    public static readonly EState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EState ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly EState INACTIVE = new(nameof(INACTIVE), 0);
    public static readonly EState DRAFT = new(nameof(DRAFT), 0);
    public static readonly EState DELETED = new(nameof(DELETED), 0);
    public static readonly EState OUT_OF_STOCK = new(nameof(OUT_OF_STOCK), 0);
    public static readonly EState IN_STOCK = new(nameof(IN_STOCK), 0);
    public static readonly EState RUNNING_OUT = new(nameof(RUNNING_OUT), 0);
}
