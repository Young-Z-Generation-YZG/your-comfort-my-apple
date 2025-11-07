using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Products.ProductModels.Events;

namespace YGZ.Catalog.Application.ProductModels.Events;

public class ProductModelCreatedDomainEventHandler : INotificationHandler<ProductModelCreatedDomainEvent>
{
    private readonly ILogger<ProductModelCreatedDomainEventHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _repository;

    public ProductModelCreatedDomainEventHandler(ILogger<ProductModelCreatedDomainEventHandler> logger,
                                                 IMongoRepository<ProductModel, ModelId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(ProductModelCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _repository.InsertOneAsync(notification.ProductModel);
    }
}
