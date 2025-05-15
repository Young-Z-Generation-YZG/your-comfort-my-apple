

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Catalog.Application.Promotions.Commands.CreatePromotionGlobal;

public class CreatePromotionGlobalCommandHandler : ICommandHandler<CreatePromotionGlobalCommand, bool>
{
    public async Task<Result<bool>> Handle(CreatePromotionGlobalCommand request, CancellationToken cancellationToken)
    {
        return true;
    }
}
