
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
using YGZ.Catalog.Application.Core.Abstractions.EventBus;
using YGZ.Catalog.Application.Products.Events;
using Microsoft.Extensions.Logging;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, bool>
{
    private readonly IProductService _productService;
    private readonly IProductItemService _productItemService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public CreateProductCommandHandler(IProductService productService, IProductItemService productItemService, IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _productService = productService;
        _productItemService = productItemService;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task<Result<bool>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.CreateUnique();

        var product = Product.Create(
            productId: productId,
            name: request.Name,
            description: request.Description,
            images: request.Images.ConvertAll(image => Image.Create(image.Url, image.Id)),
            valueRating: request.AverageRating.Value,
            numsRating: request.AverageRating.NumRatings,
            models: request.Models,
            colors: request.Colors,
            categoryId: CategoryId.ToObjectId(request.CategoryId),
            promotionId: PromotionId.ToObjectId(request.PromotionId)
        );

        await _productService.InsertOneAsync(product, null!, cancellationToken);

        try
        {
            var result = await _unitOfWork.CommitAsync(null);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Errors.Product.CannotBeCreated;
        }

        //await _eventBus.PublishAsync(new ProductCreatedEvent
        //{
        //    ProductId = "123",
        //    ProductName = "test",
        //    Description = "test"
        //}, cancellationToken);
    }
}
