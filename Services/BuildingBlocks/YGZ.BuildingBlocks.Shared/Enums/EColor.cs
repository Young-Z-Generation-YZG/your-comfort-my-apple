using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EColor : SmartEnum<EColor>
{
    public EColor(string name, int value) : base(name, value) { }

    public static readonly EColor UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EColor ULTRAMARINE = new(nameof(ULTRAMARINE), 0);
    public static readonly EColor BLUE = new(nameof(BLUE), 0);
    public static readonly EColor TEAL = new(nameof(TEAL), 0);
    public static readonly EColor PINK = new(nameof(PINK), 0);
    public static readonly EColor YELLOW = new(nameof(YELLOW), 0);
    public static readonly EColor GREEN = new(nameof(GREEN), 0);
    public static readonly EColor WHITE = new(nameof(WHITE), 0);
    public static readonly EColor BLACK = new(nameof(BLACK), 0);
    public static readonly EColor DESERT_TITANIUM = new(nameof(DESERT_TITANIUM), 0);
    public static readonly EColor WHITE_TITANIUM = new(nameof(WHITE_TITANIUM), 0);
    public static readonly EColor BLACK_TITANIUM = new(nameof(BLACK_TITANIUM), 0);
    public static readonly EColor LAVENDER = new(nameof(LAVENDER), 0);
    public static readonly EColor SAGE = new(nameof(SAGE), 0);
    public static readonly EColor MIST_BLUE = new(nameof(MIST_BLUE), 0);
    public static readonly EColor SILVER = new(nameof(SILVER), 0);
    public static readonly EColor COSMIC_ORANGE = new(nameof(COSMIC_ORANGE), 0);
    public static readonly EColor DEEP_BLUE = new(nameof(DEEP_BLUE), 0);
    public static readonly EColor SKY_BLUE = new(nameof(SKY_BLUE), 0);
    public static readonly EColor LIGHT_GOLD = new(nameof(LIGHT_GOLD), 0);
    public static readonly EColor CLOUD_WHITE = new(nameof(CLOUD_WHITE), 0);
    public static readonly EColor SPACE_BLACK = new(nameof(SPACE_BLACK), 0);

}
