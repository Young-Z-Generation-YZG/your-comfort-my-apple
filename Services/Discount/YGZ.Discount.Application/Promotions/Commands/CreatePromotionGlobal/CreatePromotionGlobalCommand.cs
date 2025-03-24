
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;

public sealed record CreatePromotionGlobalCommand() : ICommand<bool>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public PromotionGlobalType PromotionGlobalType { get; set; }
    public string PromotionEventId { get; set; }
}
