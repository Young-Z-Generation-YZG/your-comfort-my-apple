using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Categories.Commands.AddSubCategory;

public sealed record AddSubCategoryCommand : ICommand<bool>
{
}
