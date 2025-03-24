
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;

public class CreatePromotionItemCommandHandler : ICommandHandler<CreatePromotionItemCommand, bool>
{
    public async Task<Result<bool>> Handle(CreatePromotionItemCommand request, CancellationToken cancellationToken)
    {
        return true;
    }
}
