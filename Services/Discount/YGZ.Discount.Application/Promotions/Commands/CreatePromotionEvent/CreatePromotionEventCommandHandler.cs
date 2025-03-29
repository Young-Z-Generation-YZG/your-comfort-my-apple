

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Application.PromotionCoupons.Extensions;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.PromotionEvent;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Application.PromotionCoupons.Commands.CreatePromotionEvent;

public class CreatePromotionEventCommandHandler : ICommandHandler<CreatePromotionEventCommand, bool>
{
    private readonly IGenericRepository<PromotionEvent, PromotionEventId> _repository;

    public CreatePromotionEventCommandHandler(IGenericRepository<PromotionEvent, PromotionEventId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreatePromotionEventCommand request, CancellationToken cancellationToken)
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
