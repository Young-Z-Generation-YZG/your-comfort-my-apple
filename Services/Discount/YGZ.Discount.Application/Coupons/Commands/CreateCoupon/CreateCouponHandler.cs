using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Discount.Domain.Abstractions.Data;
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

        var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(request.DiscountType);
        var productClassification = ConvertGrpcEnumToNormalEnum.ConvertToEProductClassification(request.ProductClassification);

        var coupon = Coupon.Create(couponId: CouponId.Create(),
                                   userId: request.UserId,
                                   code: Code.Of(request.UniqueCode),
                                   title: request.Title,
                                   description: request.Description,
                                   productClassification: productClassification,
                                   discountState: EDiscountState.ACTIVE,
                                   discountType: discountType,
                                   discountValue: request.DiscountValue,
                                   maxDiscountAmount: request.MaxDiscountAmount,
                                   availableQuantity: request.AvailableQuantity,
                                   stock: request.Stock);

        var result = await _repository.AddAsync(coupon, cancellationToken);

        return result.Response;
    }
}
