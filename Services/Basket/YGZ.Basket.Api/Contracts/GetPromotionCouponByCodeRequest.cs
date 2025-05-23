namespace YGZ.Basket.Api.Contracts;

public sealed record GetPromotionCouponByCodeRequest
{
    public string? _couponCode { get; set; } = null;
}
