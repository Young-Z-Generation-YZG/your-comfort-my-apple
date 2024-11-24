

using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Application.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery() : IQuery<IEnumerable<ProductResponse>> { }
