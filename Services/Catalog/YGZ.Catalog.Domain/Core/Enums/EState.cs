
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EState : SmartEnum<EState>
{
    public EState(string name, int value) : base(name, value) { }

    public static readonly State UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly State ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly State INACTIVE = new(nameof(INACTIVE), 0);
    public static readonly State DRAFT = new(nameof(DRAFT), 0);
    public static readonly State DELETED = new(nameof(DELETED), 0);
    public static readonly State OUT_OF_STOCK = new(nameof(OUT_OF_STOCK), 0);
    public static readonly State IN_STOCK = new(nameof(IN_STOCK), 0);
    public static readonly State RUNNING_OUT = new(nameof(RUNNING_OUT), 0);
}
