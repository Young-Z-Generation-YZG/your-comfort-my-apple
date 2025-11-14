using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand : ICommand<bool>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int Order { get; init; }
    public string? ParentId { get; init; }
}
