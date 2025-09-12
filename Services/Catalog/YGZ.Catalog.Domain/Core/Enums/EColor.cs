
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EColor : SmartEnum<EColor>
{
    public EColor(string name, int value) : base(name, value) { }

    public static readonly EColor UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EColor BLUE = new(nameof(BLUE), 1);
    public static readonly EColor PINK = new(nameof(PINK), 2);
    public static readonly EColor YELLOW = new(nameof(YELLOW), 3);
    public static readonly EColor GREEN = new(nameof(GREEN), 4);
    public static readonly EColor BLACK = new(nameof(BLACK), 5);
}
