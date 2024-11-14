

using DnsClient.Internal;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Events;

namespace YGZ.Catalog.Application.Products.Events;

public class ProductItemCreatedEventHandler : INotificationHandler<ProductItemCreatedEvent>
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductItemCreatedEventHandler> _logger;

    public ProductItemCreatedEventHandler(IProductService productService, IUnitOfWork unitOfWork, ILogger<ProductItemCreatedEventHandler> logger)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(ProductItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        var result = await _productService.AddProductItem(notification.productItem.ProductId.ValueStr, notification.productItem);

        if (result.Response == false) {
            _logger.LogError(Errors.Product.CannotBeAddProductItem.Message);
        }
    }
}
