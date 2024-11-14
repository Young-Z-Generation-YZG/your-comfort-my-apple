
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IProductService _productService;

    public GetAllProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<IEnumerable<ProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);

        var responses = products.Select(p => new ProductResponse(
            p.Id.ValueStr,
            p.Name,
            p.Description,
            p.Models,
            p.Colors,
            new AverageRatingResponse(p.AverageRatingValue, p.AverageRatingNumRatings),
            p.ProductItems.Select(pi => new ProductItemResponse(
                pi.Sku.Value,
                pi.Model,
                pi.Color,
                pi.Storage,
                pi.QuantityInStock,
                pi.Price,
                pi.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId)).ToList(),
                pi.CreatedAt,
                pi.UpdatedAt
            )).ToList(),
            p.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId)).ToList(),
            p.Slug.Value,
            p.CategoryId.ValueStr,
            p.PromotionId.ValueStr,
            p.CreatedAt,
            p.UpdatedAt
        )).ToList();

        return Result<IEnumerable<ProductResponse>>.Success(responses);
    }
}
