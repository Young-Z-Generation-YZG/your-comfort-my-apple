

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Products.Queries.GetProductBySlug;

public sealed record GetProductBySlugQuery(string Slug) : IQuery<bool>;