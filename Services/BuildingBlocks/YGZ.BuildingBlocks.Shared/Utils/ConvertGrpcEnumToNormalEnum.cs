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
            case "DiscountTypeFixedAmount":
                return EDiscountType.FIXED_AMOUNT;
            default:
                return EDiscountType.UNKNOWN;
        }
    }

    public static ECategoryType ConvertToECategoryType(string ECategoryTypeGrpc)
    {
        switch (ECategoryTypeGrpc)
        {
            case "CategoryTypeIphone":
                return ECategoryType.IPHONE;
            case "CategoryTypeIpad":
                return ECategoryType.IPAD;
            case "CategoryTypeMacbook":
                return ECategoryType.MACBOOK;
            case "CategoryTypeWatch":
                return ECategoryType.WATCH;
            case "CategoryTypeHeadphone":
                return ECategoryType.HEADPHONE;
            case "CategoryTypeAccessory":
                return ECategoryType.ACCESSORY;
            default:
                return ECategoryType.UNKNOWN;
        }
    }
}
