using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand : ICommand<bool>
{
}
