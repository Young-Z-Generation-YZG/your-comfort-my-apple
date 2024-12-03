
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
            p.CategoryId.ValueStr,
            p.PromotionId.ValueStr,
            p.Name,
            p.Description,
            p.Models.ConvertAll(model => new ModelResponse(model.Name, model.Order)),
            p.Colors.ConvertAll(color => new ColorResponse(color.Name, color.ColorHash, color.Order)),
            p.Storages.ConvertAll(storage => new StorageResponse(storage.Name, storage.Value)),
            new AverageRatingResponse(p.AverageRating.AverageValue, p.AverageRating.NumRatings),
            p.StarRatings.Select(sr => new StarRatingResponse(sr.Star, sr.NumRatings)).ToList(),
            p.ProductItems.Select(pi => new ProductItemResponse(
                pi.Sku.Value,
                pi.Model,
                pi.Color,
                pi.Storage,
                pi.Description,
                pi.QuantityRemain,
                pi.QuantityInStock,
                pi.Sold,
                pi.Price,
                pi.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId, i.Order)).ToList(),
                pi.State.Name,
                pi.CreatedAt,
                pi.UpdatedAt
            )).ToList(),
            p.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId, i.Order)).ToList(),
            p.Slug.Value,
            p.State.Name,
            p.CreatedAt,
            p.UpdatedAt
        )).ToList();

        return Result<IEnumerable<ProductResponse>>.Success(responses);
    }
}
