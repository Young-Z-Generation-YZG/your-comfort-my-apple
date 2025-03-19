

using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class State : SmartEnum<State>
{
    public State(string name, int value) : base(name, value) { }

    public static readonly State ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly State INACTIVE = new(nameof(INACTIVE), 1);
    public static readonly State DRAFT = new(nameof(DRAFT), 2);
    public static readonly State DELETED = new(nameof(DELETED), 3);
    public static readonly State OUT_OF_STOCK = new(nameof(OUT_OF_STOCK), 4);
    public static readonly State IN_STOCK = new(nameof(IN_STOCK), 5);
    public static readonly State RUNNING_OUT = new(nameof(RUNNING_OUT), 6);
}
