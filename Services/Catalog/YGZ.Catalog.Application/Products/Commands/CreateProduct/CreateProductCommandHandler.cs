
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Domain.Promotions.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductService _productService;
    private readonly IProductItemService _productItemService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductService productService, IProductItemService productItemService ,IUnitOfWork unitOfWork)
    {
        _productService = productService;
        _productItemService = productItemService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //var uploadResult = await _uploadSerivce.UploadImageFileAsync(request.File);

        if(request.CategoryId is not null)
        {
            var categoryId = CategoryId.ToObjectId(request.CategoryId);

            if (categoryId is null)
            {
                return Errors.Category.CategoryIdInvalid;
            }
        }

        if(request.PromotionId is not null)
        {
            var promotionId = PromotionId.ToObjectId(request.PromotionId);

            if (promotionId is null)
            {
                return Errors.Promotion.PromotionIdInvalid;
            }
        }

        var productId = ProductId.CreateUnique();

        var product = Product.Create(
            productId: productId,
            name: request.Name,
            description: request.Description,
            images: request.Images.ConvertAll(image => Image.Create(image.Url, image.Id)),
            valueRating: request.AverageRating.Value,
            numsRating: request.AverageRating.NumRatings,
            productItems: request.ProductItems.ConvertAll(item => ProductItem.Create(
                productId: productId,
                model: item.Model,
                color: item.Color,
                storage: item.Storage,
                price: item.Price,
                quantityInStock: item.Quantity_in_stock,
                images: item.Images.ConvertAll(image => Image.Create(image.Url, image.Id))
                )), 
            categoryId: CategoryId.ToObjectId(request.CategoryId),
            promotionId: PromotionId.ToObjectId(request.PromotionId)
            ); 

        await _productService.CreateProductAsync(product);

        await _productItemService.CreateProductItemAsync(product.ProductItems[0]);

        try
        {
            var test = await _unitOfWork.Commit();
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Errors.Product.ProductCannotBeCreated;
        }

        return product;
    }
}
