﻿

using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Application.Products.Queries.GetProductBySlug;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.Products.Queries.GetProductById;

public class GetProductBySlugQueryHandler : IQueryHandler<GetProductBySlugQuery, ProductResponse>
{
    private readonly IProductService _productService;

    public GetProductBySlugQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<ProductResponse>> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var result = await _productService.GetBySlug(request.Slug, cancellationToken);

        if (result is null)
        {
            return Errors.Product.DoesNotExist;
        }

        var product = result.Response!;

        var response = new ProductResponse(product.Id.ValueStr,
                                           product.CategoryId.ValueStr,
                                           product.PromotionId.ValueStr,
                                           product.Name,
                                           product.Description,
                                           product.Models.ConvertAll(model => new ModelResponse(model.Name, model.Order)),
                                           product.Colors.ConvertAll(color => new ColorResponse(color.Name, color.ColorHash, color.ImageColorUrl, color.Order)),
                                           product.Storages.ConvertAll(storage => new StorageResponse(storage.Name, storage.Value)),
                                           new AverageRatingResponse(product.AverageRating.AverageValue, product.AverageRating.NumRatings),
                                           product.ProductItems.Select(pi => new ProductItemResponse(
                                               pi.Sku.Value,
                                               pi.Model,
                                               pi.Color,
                                               pi.Storage,
                                               pi.QuantityRemain, 
                                               pi.QuantityInStock, 
                                               pi.Sold,
                                               pi.Price,
                                               pi.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId)).ToList(),
                                               pi.State.Name,
                                               pi.CreatedAt,
                                               pi.UpdatedAt)).ToList(),
                                           product.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId)).ToList(),
                                           product.Slug.Value,
                                           product.State,
                                           product.CreatedAt,
                                           product.UpdatedAt);

        return response;
    }
}