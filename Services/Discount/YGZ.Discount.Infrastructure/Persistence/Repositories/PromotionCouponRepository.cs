using YGZ.Discount.Domain.Abstractions.Data;

namespace YGZ.Discount.Infrastructure.Persistence.Repositories;

public class PromotionCouponRepository : IPromotionCouponRepository
{
    private readonly DiscountDbContext _context;

    public PromotionCouponRepository(DiscountDbContext context)
    {
        _context = context;
    }

    //public async Task<List<Coupon>> GetAllAsync(CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var result = await _context.Coupons
    //            .Where(c => !c.IsDeleted)
    //            .ToListAsync(cancellationToken);

    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new RpcException(new Status(StatusCode.Internal, ex.Message));
    //    }
    //}

    //public async Task<Coupon?> GetByCode(Code code, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var result = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted);

    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new RpcException(new Status(StatusCode.Internal, ex.Message));
    //    }
    //}
}
