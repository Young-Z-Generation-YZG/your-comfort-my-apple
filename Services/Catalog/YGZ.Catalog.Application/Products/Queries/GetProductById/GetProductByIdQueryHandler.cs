﻿

using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductService _productService;

    public GetProductByIdQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productService.FindByIdAsync(request.Id, cancellationToken);

        if(product is null)
        {
            return Errors.Product.DoesNotExist;
        }

        var response = new ProductResponse(product.Id.ValueStr,
                                           product.CategoryId.ValueStr,
                                           product.PromotionId.ValueStr,
                                           product.Name,
                                           product.Description,
                                           product.Models.ConvertAll(model => new ModelResponse(model.Name, model.Order)),
                                           product.Colors.ConvertAll(color => new ColorResponse(color.Name, color.ColorHash, color.Order)),
                                           product.Storages.ConvertAll(storage => new StorageResponse(storage.Name, storage.Value)),
                                           new AverageRatingResponse(product.AverageRating.AverageValue, product.AverageRating.NumRatings),
                                           product.StarRatings.Select(sr => new StarRatingResponse(sr.Star, sr.NumRatings)).ToList(),
                                           product.ProductItems.Select(pi => new ProductItemResponse(
                                               pi.Id.Value.ToString(),
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
                                               pi.UpdatedAt)).ToList(),
                                           product.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId, i.Order)).ToList(),
                                           product.Slug.Value,
                                           product.State.Name,
                                           product.CreatedAt,
                                           product.UpdatedAt);

        return response;
    }
}
