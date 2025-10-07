using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand : ICommand<bool>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Order { get; set; } = default!;
    public string? ParentId { get; set; } = null;
}
