

using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Domain.Abstractions.Data;

public interface IPromotionCouponRepository
{
    Task<Coupon?> GetByCode(Code code, CancellationToken cancellationToken);
    Task<List<Coupon>> GetAllAsync(CancellationToken cancellationToken);
}
