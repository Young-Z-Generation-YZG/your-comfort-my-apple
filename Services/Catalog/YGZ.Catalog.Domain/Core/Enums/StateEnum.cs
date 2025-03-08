

using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class StateEnum : SmartEnum<StateEnum>
{
    public StateEnum(string name, int value) : base(name, value) { }

    public static readonly StateEnum ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly StateEnum INACTIVE = new(nameof(INACTIVE), 1);
    public static readonly StateEnum DRAFT = new(nameof(DRAFT), 3);
    public static readonly StateEnum DELETED = new(nameof(DELETED), 2);
}
