


using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Detail;

public sealed record CreateIPhone16DetailsCommand : ICommand<bool>
{
    public string Model { get; set; } = default!;
    public ColorCommand Color { get; set; } = default!;
    public int Storage { get; set; } = 0;
    public decimal UnitPrice { get; set; } = 0;
    public string Description { get; set; } = default!;
    public ImageCommand[] Images { get; set; } = default!;
    public string IPhoneModelId { get; set; } = default!;
}