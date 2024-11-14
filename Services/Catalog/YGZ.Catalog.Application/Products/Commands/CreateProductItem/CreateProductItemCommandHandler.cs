
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Products.Commands.CreateProductItem;

public class CreateProductItemCommandHandler : ICommandHandler<CreateProductItemCommand, ProductItem>
{
    private readonly IProductService _productService;
    private readonly IProductItemService _productItemService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductItemCommandHandler(IProductItemService productItemService, IUnitOfWork unitOfWork, IProductService productService)
    {
        _productItemService = productItemService;
        _unitOfWork = unitOfWork;
        _productService = productService;
    }

    public async Task<Result<ProductItem>> Handle(CreateProductItemCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.FindByIdAsync(request.ProductId!, cancellationToken);

        if (product is null)
        {
            return Errors.Product.DoesNotExist;
        }

        var productItem = ProductItem.Create(product.Id,
                                             request.Model,
                                             request.Color,
                                             request.Storage,
                                             request.Price,
                                             request.Quantity_in_stock,
                                             request.Images.ConvertAll(image => Image.Create(image.Url, image.Id)));

        await _productItemService.InsertOneAsync(productItem, null!, cancellationToken);

        try
        {
            await _unitOfWork.Commit();
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Errors.ProductItem.CannotBeCreated;
        }

        return productItem;
    }
}
