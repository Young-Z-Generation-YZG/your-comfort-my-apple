using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EIphoneModel : SmartEnum<EIphoneModel>
{
    public EIphoneModel(string name, int value) : base(name, value) { }

    public static readonly EIphoneModel UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EIphoneModel IPHONE_15 = new(nameof(IPHONE_15), 0);
    public static readonly EIphoneModel IPHONE_15_PLUS = new(nameof(IPHONE_15_PLUS), 0);
    public static readonly EIphoneModel IPHONE_16 = new(nameof(IPHONE_16), 0);
    public static readonly EIphoneModel IPHONE_16_PLUS = new(nameof(IPHONE_16_PLUS), 0);
    public static readonly EIphoneModel IPHONE_16E = new(nameof(IPHONE_16E), 0);
    public static readonly EIphoneModel IPHONE_17 = new(nameof(IPHONE_17), 0);
    public static readonly EIphoneModel IPHONE_17_PRO = new(nameof(IPHONE_17_PRO), 0); 
    public static readonly EIphoneModel IPHONE_17_PRO_MAX = new(nameof(IPHONE_17_PRO_MAX), 0); 
    public static readonly EIphoneModel IPHONE_17_AIR = new(nameof(IPHONE_17_AIR), 0);
}
