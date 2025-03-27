

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Model;

public sealed record CreateIPhone16ModelCommand : ICommand<bool>
{
    required public string Name { get; set; }
    required public List<IPhoneModelCommand> Models { get; set; }
    required public List<ColorCommand> Colors { get; set; }
    required public List<int> Storages { get; set; }
    public string Description { get; set; } = default!;
    public List<ImageCommand> DescriptionImages { get; set; } = default!;
    public string? CategoryId { get; set; } = default!;
}
