

using System.Windows.Input;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Categories.Commands;

public sealed record CreateCategoryCommand : ICommand<bool>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ParentId { get; set; } = null;
}
