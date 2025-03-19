

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Model;

public sealed record CreateIPhone16ModelCommand : ICommand<bool>
{
    public List<IPhoneModelCommand> Models { get; set; } = default!;
    public List<ColorCommand> Colors { get; set; } = default!;
    public List<int> Storages { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<ImageCommand> DescriptionImages { get; set; } = default!;
    public string? CategoryId { get; set; } = default!;
}
