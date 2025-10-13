
using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Queries.GetAllPromotionCoupons;

public class GetCouponsHandler : IQueryHandler<GetCouponsQuery, List<CouponResponse>>
{
    private readonly IGenericRepository<Coupon, CouponId> _repository;

    public GetCouponsHandler(IGenericRepository<Coupon, CouponId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<CouponResponse>>> Handle(GetCouponsQuery request, CancellationToken cancellationToken)
    {
        // Get all non-deleted coupons
        var coupons = await _repository.DbSet
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        // Map to response DTOs
        var couponResponses = coupons
            .Select(c => c.ToResponse())
            .ToList();

        return couponResponses;
    }
}
