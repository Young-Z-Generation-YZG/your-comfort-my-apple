

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Application.PromotionCoupons.Extensions;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;

public class CreatePromotionCategoryCommandHandler : ICommandHandler<CreatePromotionCategoryCommand, bool>
{
    private readonly IGenericRepository<PromotionCategory, CategoryId> _repository;

    public CreatePromotionCategoryCommandHandler(IGenericRepository<PromotionCategory, CategoryId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreatePromotionCategoryCommand request, CancellationToken cancellationToken)
    {
        var entities = request.PromotionCategories.Select(x => x.ToEntity()).ToList();

        var result = await _repository.AddRangeAsync(entities, cancellationToken);

        if(result.IsFailure)
        {
            return false;
        }

        return true;
    }
}
