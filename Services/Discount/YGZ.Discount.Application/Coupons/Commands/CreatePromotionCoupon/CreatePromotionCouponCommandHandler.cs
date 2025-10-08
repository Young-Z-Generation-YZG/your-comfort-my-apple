
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Commands.CreateCoupon;

public class CreatePromotionCouponCommandHandler : ICommandHandler<CreatePromotionCouponCommand, bool>
{
    private readonly IGenericRepository<Coupon, CouponId> _repository;

    public CreatePromotionCouponCommandHandler(IGenericRepository<Coupon, CouponId> couponRepository)
    {
        _repository = couponRepository;
    }

    public async Task<Result<bool>> Handle(CreatePromotionCouponCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var entity = request.ToEntity();

        //var result = await _repository.AddAsync(entity, cancellationToken);

        //if(result.IsFailure)
        //{
        //    return false;
        //}

        //return true;
    }
}
