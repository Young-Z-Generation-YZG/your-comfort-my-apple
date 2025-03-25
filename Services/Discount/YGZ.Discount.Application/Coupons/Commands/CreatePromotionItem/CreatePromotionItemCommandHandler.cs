
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Application.PromotionCoupons.Extensions;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.PromotionItem;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;

public class CreatePromotionItemCommandHandler : ICommandHandler<CreatePromotionItemCommand, bool>
{
    private readonly IGenericRepository<PromotionItem, PromotionItemId> _repository;

    public CreatePromotionItemCommandHandler(IGenericRepository<PromotionItem, PromotionItemId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreatePromotionItemCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity();

        var result = await _repository.AddAsync(entity, cancellationToken);

        if(result.IsFailure)
        {
            return false;
        }

        return true;
    }
}
