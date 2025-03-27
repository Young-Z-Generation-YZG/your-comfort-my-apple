

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Coupons.Queries.GetAllCoupons;

public class GetAllCouponsQueryHandler : IQueryHandler<GetAllCouponsQuery, PaginationResponse<GetCouponResponse>>
{
    public Task<Result<PaginationResponse<GetCouponResponse>>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
