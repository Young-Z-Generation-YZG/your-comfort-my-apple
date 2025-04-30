
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Promotions.Commands.UpdatePromotionEvent;

public sealed record UpdateEventCommand : ICommand<bool>
{
}
