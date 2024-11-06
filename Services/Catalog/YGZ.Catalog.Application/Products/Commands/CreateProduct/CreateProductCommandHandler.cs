
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

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductService productService, IUnitOfWork unitOfWork)
    {
        _productService = productService;
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
            categoryId: CategoryId.ToObjectId(request.CategoryId),
            promotionId: PromotionId.ToObjectId(request.PromotionId)
            ); 

        var result = await _productService.CreateProductAsync(product);


        await _unitOfWork.Commit();

        if (result.IsFailure)
        {
            return result.Error;
        }

        return product;
    }
}
