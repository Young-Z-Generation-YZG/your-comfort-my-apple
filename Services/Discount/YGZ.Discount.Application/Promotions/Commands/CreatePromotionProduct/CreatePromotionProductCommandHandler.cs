

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Application.PromotionCoupons.Extensions;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;

public class CreatePromotionProductCommandHandler : ICommandHandler<CreatePromotionProductCommand, bool>
{
    private readonly IGenericRepository<PromotionProduct, ProductId> _repository;

    public CreatePromotionProductCommandHandler(IGenericRepository<PromotionProduct, ProductId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreatePromotionProductCommand request, CancellationToken cancellationToken)
    {
        var entities = request.ProductPromotions.Select(x => x.ToEntity()).ToList();

        var result = await _repository.AddRangeAsync(entities, cancellationToken);

        if (result.IsFailure)
        {
            return false;
        }

        return true;
    }
}
