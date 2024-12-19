
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.ProductItems.Queries.GetProductItemById;

public class GetProductItemByIdQueryHandler : IQueryHandler<GetProductItemByIdQuery, ProductItemResponse>
{
    private readonly IProductItemService _productItemService;

    public GetProductItemByIdQueryHandler(IProductItemService productItemService)
    {
        _productItemService = productItemService;
    }

    public async Task<Result<ProductItemResponse>> Handle(GetProductItemByIdQuery request, CancellationToken cancellationToken)
    {
        var productItem = await _productItemService.FindByIdAsync(request.Id, cancellationToken);

        if(productItem is null)
        {
            return Errors.ProductItem.DoesNotExist;
        }

        var response = new ProductItemResponse(productItem.Id.Value.ToString()!,
                                               productItem.Sku.Value,
                                               productItem.Model,
                                               productItem.Color,
                                               productItem.Storage,
                                               productItem.Description,
                                               productItem.QuantityRemain,
                                               productItem.QuantityInStock,
                                               productItem.Sold,
                                               productItem.Price,
                                               productItem.Images.Select(i => new ImageResponse(i.ImageUrl, i.ImageId, i.Order)).ToList(),
                                               productItem.State.Name,
                                               productItem.CreatedAt,
                                               productItem.UpdatedAt
                                               );

        return response;
    }
}
