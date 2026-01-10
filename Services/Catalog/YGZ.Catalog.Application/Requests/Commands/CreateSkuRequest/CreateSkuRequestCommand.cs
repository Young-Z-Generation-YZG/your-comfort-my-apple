using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Requests.Commands.CreateSkuRequest;

public sealed record CreateSkuRequestCommand : ICommand<bool>
{
    public required string SenderUserId { get; init; }
    public required string FromBranchId { get; init; }
    public required string ToBranchId { get; init; }
    public required string SkuId { get; init; }
    public int RequestQuantity { get; init; }
}
