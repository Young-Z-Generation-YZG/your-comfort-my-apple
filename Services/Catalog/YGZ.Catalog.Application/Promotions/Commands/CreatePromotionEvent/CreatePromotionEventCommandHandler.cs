

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Catalog.Application.Promotions.Commands.CreatePromotionEvent;

public class CreatePromotionEventCommandHandler : ICommandHandler<CreatePromotionEventCommand, bool>
{
    public async Task<Result<bool>> Handle(CreatePromotionEventCommand request, CancellationToken cancellationToken)
    {
        return true;
    }
}
