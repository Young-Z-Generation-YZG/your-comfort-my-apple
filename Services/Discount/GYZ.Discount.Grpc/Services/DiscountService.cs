using Grpc.Core;
using GYZ.Discount.Grpc.Data;
using GYZ.Discount.Grpc.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GYZ.Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountDbContext _dbContext;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(DiscountDbContext dbContext, ILogger<DiscountService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if(coupon == null)
        {
            _logger.LogError($"Discount with ProductName={request.ProductName} is not found.");

            coupon = new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };
            //throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
        }

        _logger.LogInformation($"Discount is retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon  = request.Coupon.Adapt<Coupon>();

        if(coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Data"));
        }

        _dbContext.Coupons.Add(coupon);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Discount is successfully created. ProductName: {coupon.ProductName}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Data"));
        }

        _dbContext.Coupons.Update(coupon);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Discount is successfully created. ProductName: {coupon.ProductName}");

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
        }

        _dbContext.Coupons.Remove(coupon);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Discount is successfully deleted. ProductName: {coupon.ProductName}");

        return new DeleteDiscountResponse
        {
            Success = true
        };
    }
}
