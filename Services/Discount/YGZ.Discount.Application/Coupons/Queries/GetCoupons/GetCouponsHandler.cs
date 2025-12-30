
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Queries.GetAllPromotionCoupons;

public class GetCouponsHandler : IQueryHandler<GetCouponsQuery, List<CouponResponse>>
{
    private readonly ILogger<GetCouponsHandler> _logger;
    private readonly IGenericRepository<Coupon, CouponId> _repository;

    public GetCouponsHandler(IGenericRepository<Coupon, CouponId> repository, ILogger<GetCouponsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
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

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved coupons", new { couponCount = couponResponses.Count });

        return couponResponses;
    }
}
