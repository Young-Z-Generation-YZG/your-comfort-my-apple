

using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Application.ProductItems.Queries.GetProductItemById;

public record GetProductItemByIdQuery(string Id) : IQuery<ProductItemResponse> { }

