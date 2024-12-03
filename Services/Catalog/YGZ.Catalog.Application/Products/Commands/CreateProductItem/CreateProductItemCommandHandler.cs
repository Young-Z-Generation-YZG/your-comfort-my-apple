
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Application.Core.Abstractions.Services;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Abstractions.Common;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Application.Products.Commands.CreateProductItem;

public class CreateProductItemCommandHandler : ICommandHandler<CreateProductItemCommand, bool>
{
    private readonly IProductService _productService;
    private readonly IProductItemService _productItemService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductItemCommandHandler(IProductItemService productItemService, IUnitOfWork unitOfWork, IProductService productService, IDateTimeProvider dateTimeProvider)
    {
        _productItemService = productItemService;
        _unitOfWork = unitOfWork;
        _productService = productService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<bool>> Handle(CreateProductItemCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.FindByIdAsync(request.ProductId!, cancellationToken);
    
        if (product is null)
        {
            return Errors.Product.DoesNotExist;
        }

        if (!product.Models.Any(model => model.Name == request.Model))
        {
            return Errors.Product.InvalidModel(product.Models.ConvertAll(model => model.Name));
        }

        if (!product.Colors.Any(model => model.Name == request.Color))
        {
            return Errors.Product.InvalidColor(product.Colors.ConvertAll(model => model.Name));
        }

        var productItem = ProductItem.Create(ProductItemId.CreateUnique(), 
                                             product.Id,
                                             request.Model,
                                             request.Color,
                                             request.Storage,
                                             request.Price,
                                             request.Quantity_in_stock,
                                             request.Images.ConvertAll(image => Image.Create(image.Url, image.Id, image.Order)),
                                             _dateTimeProvider.Now
                                             );

        await _productItemService.InsertOneAsync(productItem, null!, cancellationToken);

        try
        {
            var eventDomains = new List<IHasDomainEvents> { productItem };

            var result = await _unitOfWork.CommitAsync(eventDomains);

            return result;
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Errors.ProductItem.CannotBeCreated;
        }
    }
}
