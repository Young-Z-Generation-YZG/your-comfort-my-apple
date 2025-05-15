
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Promotions.Commands.CreatePromotionEvent;

public sealed record CreatePromotionEventCommand : ICommand<bool>
{
    public required string EventTitle { get; set; }
    public required string EventDescription { get; set; }
    public required string EventState { get; set; }
    public required DateTime EventValidFrom { get; set; }
    public required DateTime EventValidTo { get; set; }
}
