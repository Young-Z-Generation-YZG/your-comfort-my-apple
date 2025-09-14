
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Application.IPhone.Commands.CreateIphoneModel;

public sealed record CreateIphoneModelCommand : ICommand<bool>
{
    required public string Name { get; init; }
    required public List<IPhoneModelCommand> Models { get; init; }
    required public List<ColorCommand> Colors { get; init; }
    required public List<StorageCommand> Storages { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<ImageCommand> ShowcaseImages { get; init; } = [];
    public string? CategoryId { get; init; }
}
