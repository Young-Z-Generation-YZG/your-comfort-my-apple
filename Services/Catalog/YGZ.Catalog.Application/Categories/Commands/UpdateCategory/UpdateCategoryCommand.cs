using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand : ICommand<bool>
{
    public required string CategoryId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public int? Order { get; init; }
    public string? ParentCategoryId { get; init; }
    public List<string>? SubCategoryIds { get; init; }
}
