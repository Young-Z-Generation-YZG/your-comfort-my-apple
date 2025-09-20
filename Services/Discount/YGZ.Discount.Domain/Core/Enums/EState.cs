
using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class EState : SmartEnum<EState>
{
    public EState(string name, int value) : base(name, value) { }

    public static readonly EState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EState ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly EState INACTIVE = new(nameof(INACTIVE), 0);
    public static readonly EState EXPIRED = new(nameof(EXPIRED), 0);
}