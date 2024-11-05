
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        //var uploadResult = await _uploadSerivce.UploadImageFileAsync(request.File);

        var product = Product.Create(
            name: request.Name,
            description: request.Description,
            images: request.Images.ConvertAll(image => Image.Create(image.Url, image.Id)),
            valueRating: request.AverageRating.Value,
            numsRating: request.AverageRating.NumRatings,
            productItems: request.ProductItems.ConvertAll(item => ProductItem.Create(
                model: item.Model,
                color: item.Color,
                storage: item.Storage,
                price: item.Price,
                quantityInStock: item.Quantity_in_stock,
                images: item.Images.ConvertAll(image => Image.Create(image.Url, image.Id))
                )),
            categoryId: request.CategoryId,
            promotionId: request.PromotionId
            ); 

        var result = await _productService.CreateProductAsync(product);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return product;
    }
}
