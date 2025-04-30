

using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EndDiscountType : SmartEnum<EndDiscountType>
{
    public EndDiscountType(string name, int value) : base(name, value)
    {
    }

    public static readonly EndDiscountType UNKNOWN = new("UNKNOWN", 0);
    public static readonly EndDiscountType BY_END_DATE = new("BY_END_DATE", 1);
    public static readonly EndDiscountType BY_QUANTITY = new("BY_QUANTITY", 2);
}
