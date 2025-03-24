

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Application.PromotionCoupons.Extensions;
using YGZ.Discount.Domain.Data;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;

public class CreatePromotionGlobalCommandHandler : ICommandHandler<CreatePromotionGlobalCommand, bool>
{
    private readonly IGenericRepository<PromotionGlobal, PromotionGlobalId> _repository;

    public CreatePromotionGlobalCommandHandler(IGenericRepository<PromotionGlobal, PromotionGlobalId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreatePromotionGlobalCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity();

        var result = await _repository.AddAsync(entity, cancellationToken);

        if (result.IsFailure)
        {
            return false;
        }

        return true;
    }
}
