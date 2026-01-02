using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Commands.CreateCoupon;

public class CreateCouponHandler : ICommandHandler<CreateCouponCommand, bool>
{
    private readonly IGenericRepository<Coupon, CouponId> _repository;
    private readonly ILogger<CreateCouponHandler> _logger;

    public CreateCouponHandler(IGenericRepository<Coupon, CouponId> repository, ILogger<CreateCouponHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[CreateCouponHandler][Command:CreateCouponCommand]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "", request);

        var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(request.DiscountType);
        var productClassification = ConvertGrpcEnumToNormalEnum.ConvertToEProductClassification(request.ProductClassification);

        if (discountType.Name == EDiscountType.UNKNOWN.Name)
        {
            _logger.LogError(":::[CreateCouponHandler][Result.Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Invalid discount type", new { discountType = request.DiscountType });

            return Errors.Coupon.InvalidDiscountType;
        }
        else if (productClassification.Name == EProductClassification.UNKNOWN.Name)
        {
            _logger.LogError(":::[CreateCouponHandler][Result.Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Invalid product classification", new { productClassification = request.ProductClassification });

            return Errors.Coupon.InvalidProductClassification;
        }

        string? userId = null;

        if (!string.IsNullOrWhiteSpace(request.UserId))
        {
            userId = request.UserId;
        }


        double? maxDiscountAmount = null;

        if (request.MaxDiscountAmount.HasValue && request.MaxDiscountAmount.Value < 0)
        {
            _logger.LogError(":::[CreateCouponHandler][Result.Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Max discount amount cannot be negative", new { maxDiscountAmount = request.MaxDiscountAmount.Value });

            return Errors.Coupon.InvalidMaxDiscountAmount;
        }

        if (request.MaxDiscountAmount.HasValue && request.MaxDiscountAmount.Value > 0)
        {
            maxDiscountAmount = request.MaxDiscountAmount.Value;
        }

        if (request.Stock <= 0)
        {
            _logger.LogError(":::[CreateCouponHandler][Result.Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Stock must be greater than 0", new { stock = request.Stock });

            return Errors.Coupon.InvalidStock;
        }

        var code = Code.Of(request.CouponCode);
        var existingCoupon = await _repository.DbSet.FirstOrDefaultAsync(x => x.Code == code, cancellationToken);

        if (existingCoupon is not null)
        {
            _logger.LogError(":::[CreateCouponHandler][Result.Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Coupon code already exists", new { code = request.CouponCode });

            return Errors.Coupon.DuplicateCode;
        }

        var coupon = Coupon.Create(couponId: CouponId.Create(),
                                   userId: userId,
                                   code: code,
                                   title: request.Title,
                                   description: request.Description,
                                   productClassification: productClassification,
                                   promotionType: EPromotionType.COUPON,
                                   discountState: EDiscountState.ACTIVE,
                                   discountType: discountType,
                                   discountValue: request.DiscountValue,
                                   maxDiscountAmount: maxDiscountAmount,
                                   stock: request.Stock,
                                   expiredDate: request.ExpiredDate);

        var result = await _repository.AddAsync(coupon, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[CreateCouponHandler][Result.Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.AddAsync), "Failed to create coupon", new { code = request.CouponCode, title = request.Title, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation(":::[CreateCouponHandler][Response]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully created coupon", coupon.ToResponse());

        return result.Response;
    }
}
