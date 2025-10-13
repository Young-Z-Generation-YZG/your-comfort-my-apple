using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;
using YGZ.Discount.Domain.Core.Errors;

namespace YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;

public class GetCouponByCodeHandler : IQueryHandler<GetCouponByCodeQuery, CouponResponse>
{
    private readonly IGenericRepository<Coupon, CouponId> _repository;

    public GetCouponByCodeHandler(IGenericRepository<Coupon, CouponId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<CouponResponse>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _repository.DbSet.FirstOrDefaultAsync(x => x.Code == Code.Of(request.Code), cancellationToken);

        if (coupon is null)
        {
            return Errors.Coupon.CouponNotFound;
        }

        return coupon.ToResponse();
    }
}
