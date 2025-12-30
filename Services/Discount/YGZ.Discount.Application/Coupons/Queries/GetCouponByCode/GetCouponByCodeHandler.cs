using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;

public class GetCouponByCodeHandler : IQueryHandler<GetCouponByCodeQuery, CouponResponse>
{
    private readonly ILogger<GetCouponByCodeHandler> _logger;
    private readonly IGenericRepository<Coupon, CouponId> _repository;

    public GetCouponByCodeHandler(IGenericRepository<Coupon, CouponId> repository, ILogger<GetCouponByCodeHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<CouponResponse>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _repository.DbSet.FirstOrDefaultAsync(x => x.Code == Code.Of(request.Code), cancellationToken);

        if (coupon is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Coupon not found by code", new { code = request.Code });

            return Errors.Coupon.NotFound;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved coupon by code", new { code = request.Code, couponId = coupon.Id.ToString() });

        return coupon.ToResponse();
    }
}
