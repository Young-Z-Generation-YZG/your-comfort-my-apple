

using Grpc.Core;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandHandler : ICommandHandler<DeleteCouponCommand, bool>
{
    private readonly IGenericRepository<Coupon, CouponId> _repository;

    public DeleteCouponCommandHandler(IGenericRepository<Coupon, CouponId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _repository.GetByIdAsync(CouponId.Of(request.CouponId), cancellationToken);

        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with id:{request.CouponId} not found"));
        }

        await _repository.RemoveAsync(coupon, cancellationToken);

        return true;
    }
}
