
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Coupons;

namespace YGZ.Discount.Application.Data;

public interface IDiscountRepository
{
    Task<(List<Coupon> coupons, int TotalCount, int TotalPages)> GetAllAsync(int _page, int _limit, DiscountStateEnum? state);
    Task<Coupon> GetByCode(string code);
    Task<bool> CreateAsync(Coupon coupon);
    Task<bool> UpdateAsync(string code, Coupon coupon);
    Task<bool> DeleteAsync(string code);
}
