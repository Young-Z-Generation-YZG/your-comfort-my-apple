

using MassTransit;
using Microsoft.Extensions.Logging;

namespace YGZ.Catalog.Application.Products.Events;

public sealed class ProductCreatedEventComsumer : IConsumer<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventComsumer> _logger;

    public ProductCreatedEventComsumer(ILogger<ProductCreatedEventComsumer> logger)
    {
        _logger = logger;
    }


    public Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        _logger.LogInformation("Product created: {ProductId}, {ProductName}, {Description}", context.Message.ProductId, context.Message.ProductName, context.Message.Description);

        return Task.CompletedTask;
    }
}
