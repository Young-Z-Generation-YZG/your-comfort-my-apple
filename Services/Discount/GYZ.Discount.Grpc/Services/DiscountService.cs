using Grpc.Core;
using GYZ.Discount.Grpc.Abstractions;
using GYZ.Discount.Grpc.Data;
using GYZ.Discount.Grpc.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYZ.Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountDbContext _dbContext;
    private readonly ILogger<DiscountService> _logger;
    private readonly IUniqueCodeGenerator _uniqueCodeGenerator;

    public DiscountService(DiscountDbContext dbContext, ILogger<DiscountService> logger, IUniqueCodeGenerator uniqueCodeGenerator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _uniqueCodeGenerator = uniqueCodeGenerator;
    }

    public override async Task<CouponResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Code == request.Code);

        if (coupon == null)
        {
            _logger.LogError($"Discount with Code={request.Code} is not found.");

          
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Code={request.Code} is not found."));
        }

        _logger.LogInformation($"Discount is retrieved for Code: {coupon.Code}, Title: {coupon.Title}");

        try
        {
            var couponModel = new CouponResponse
            {
                Id = coupon.Id.ToString(),
                Code = coupon.Code,
                Title = coupon.Title,
                Type = coupon.Type.Name,
                Status = coupon.Status.Name,
                Description = coupon.Description,
                DiscountValue = coupon.DiscountValue,
                MinPurchaseAmount = null,
                MaxDiscountAmount = null,
                ValidFrom = coupon.ValidFrom.Value.ToString(),
                ValidTo = coupon.ValidTo.Value.ToString(),
                QuantityRemain = coupon.QuantityRemain,
                UsageLimit = coupon.UsageLimit,
                CreatedAt = coupon.CreatedAt.ToString(),
                UpdatedAt = coupon.UpdatedAt.ToString(),
                DeletedAt = coupon.ValidFrom.Value.ToString()
            };

            return couponModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.ToString()));
        }
    }

    public override async Task<CreateDiscountResponse> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        if (!DateTime.TryParse(request.ValidFrom, out var validFrom))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ValidFrom date format"));
        }

        if (!DateTime.TryParse(request.ValidTo, out var validTo))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ValidTo date format"));
        }

        if(validFrom > validTo)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "ValidFrom date must be less than ValidTo date"));
        }

        if(validFrom < DateTime.UtcNow)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "ValidFrom date must be greater than current date"));
        }

        var uniqueCode = _uniqueCodeGenerator.GenerateUniqueCode();

        var coupon = Coupon.CreateNew(
            uniqueCode,
            request.Title,
            request.Description,
            request.DiscountValue,
            request.MinPurchaseAmount,
            request.MaxDiscountAmount,
            validFrom,
            validTo,
            request.UsageLimit);

        if(coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Data"));
        }

        _dbContext.Coupons.Add(coupon);


        try
        {
            await _dbContext.SaveChangesAsync();

            return new CreateDiscountResponse
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            return new CreateDiscountResponse
            {
                IsSuccess = false
            };
        }
    }

    public override async Task<UpdateDiscountResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Id"));
        }

        if (!DateTime.TryParse(request.ValidFrom, out var validFrom))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ValidFrom date format"));
        }

        if (!DateTime.TryParse(request.ValidTo, out var validTo))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ValidTo date format"));
        }

        if (validFrom > validTo)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "ValidFrom date must be less than ValidTo date"));
        }

        if (validFrom < DateTime.UtcNow)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "ValidFrom date must be greater than current date"));
        }

        var coupon = Coupon.ToUpdate(id, request.Title, request.Description, request.DiscountValue, request.MinPurchaseAmount, request.MaxDiscountAmount, validFrom, validTo, request.UsageLimit);

        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Data"));
        }

        _dbContext.Coupons.Update(coupon);


        try
        {
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Discount is successfully created. Title: {coupon.Title}");

            return new UpdateDiscountResponse
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.ToString()));
        }
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Id"));
        }

        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == id);

        if (coupon == null)
        {
            _logger.LogError($"Discount with Id={request.Id} is not found.");


            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Id={request.Id} is not found."));
        }

        _logger.LogInformation($"Discount is retrieved for Id: {coupon.Id}, Title: {coupon.Title}");


        _dbContext.Coupons.Remove(coupon);

        try
        {
            await _dbContext.SaveChangesAsync();

            return new DeleteDiscountResponse
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.ToString()));
        }
    }
}
