
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;

public sealed record CreatePromotionGlobalCommand() : ICommand<bool>
{
    required public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    required public PromotionGlobalType PromotionGlobalType { get; set; }
    required public string PromotionEventId { get; set; }
}
