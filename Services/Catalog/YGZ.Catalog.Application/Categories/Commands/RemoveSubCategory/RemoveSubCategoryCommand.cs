using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Categories.Commands.RemoveSubCategory;

public sealed record RemoveSubCategoryCommand : ICommand<bool>
{
}
