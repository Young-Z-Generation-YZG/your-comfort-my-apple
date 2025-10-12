using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class ConvertGrpcEnumToNormalEnum
{
    public static EDiscountType ConvertToEDiscountType(string EDiscountTypeGrpc)
    {
        switch (EDiscountTypeGrpc)
        {
            case "DiscountTypePercentage":
                return EDiscountType.PERCENTAGE;
            case "DiscountTypeFixed":
                return EDiscountType.FIXED;
            default:
                throw new Exception("Invalid discount type");
        }

    }
}
