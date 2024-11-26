
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Application.Products.Queries.GetProductBySlug;

public sealed record GetProductBySlugQuery(string Slug) : IQuery<ProductResponse> { }
