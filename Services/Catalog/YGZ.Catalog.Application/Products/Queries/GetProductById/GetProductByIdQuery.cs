

using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(string Id) : IQuery<ProductResponse> { }