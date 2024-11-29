
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Domain.Promotions.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Domain.Products.ValueObjects;
using YGZ.Catalog.Application.Core.Abstractions.EventBus;
using YGZ.Catalog.Domain.Core.Abstractions.Common;
using YGZ.Catalog.Domain.Core.Enums;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, bool>
{
    private readonly IProductService _productService;
    private readonly IProductItemService _productItemService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateProductCommandHandler(IProductService productService, IProductItemService productItemService, IUnitOfWork unitOfWork, IEventBus eventBus, IDateTimeProvider dateTimeProvider)
    {
        _productService = productService;
        _productItemService = productItemService;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<bool>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.CreateUnique();

        var productStorages = new List<StorageEnum>();

        try
        {
            productStorages = request.Storages.ConvertAll(s => StorageEnum.FromValue(s));
        }
        catch
        {
            return Errors.Product.InvalidStorage;
        }

        var product = Product.Create(
            productId: productId,
            categoryId: CategoryId.ToObjectId(request.CategoryId),
            promotionId: PromotionId.ToObjectId(request.PromotionId),
            name: request.Name,
            models: request.Models.ConvertAll(model => Model.CreateNew(model.Name, model.Order)),
            colors: request.Colors.ConvertAll(color => Color.CreateNew(color.Name, color.ColorHash, color.ImageColorUrl, color.Order)),
            storages: productStorages,
            description: request.Description,
            images: request.Images.ConvertAll(image => Image.Create(image.Url, image.Id)),
            createdAt: _dateTimeProvider.Now
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
