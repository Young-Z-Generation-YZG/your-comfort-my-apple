
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EIphoneModel : SmartEnum<EIphoneModel>
{
    public EIphoneModel(string name, int value) : base(name, value) { }

    public static readonly EIphoneModel UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EIphoneModel IPHONE_15 = new(nameof(IPHONE_15), 1);
    public static readonly EIphoneModel IPHONE_15_PLUS = new(nameof(IPHONE_15_PLUS), 2);
}
