using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.Categories.Queries.GetCategoryDetails;

public sealed record GetCategoryDetailsQuery : IQuery<CategoryResponse>
{
    public required string CategoryId { get; init; }
}
