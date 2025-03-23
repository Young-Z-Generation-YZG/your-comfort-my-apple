
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery() : IQuery<List<CategoryResponse>>;