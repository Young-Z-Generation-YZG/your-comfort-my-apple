using MassTransit;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.ProductModels.Events.IntegrationEvents;

public class OrderDeliveredIntegrationEventHandler : IConsumer<OrderDeliveredIntegrationEvent>
{
    private readonly ILogger<OrderDeliveredIntegrationEventHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _repository;

    public OrderDeliveredIntegrationEventHandler(ILogger<OrderDeliveredIntegrationEventHandler> logger,
                                                 IMongoRepository<ProductModel, ModelId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<OrderDeliveredIntegrationEvent> context)
    {
        _logger.LogInformation("Integration event handled: {IntegrationEvent}", context.Message.GetType().Name);

        var orderItems = context.Message.OrderItems;

        // Group order items by ModelId to aggregate quantities per ProductModel
        var modelGroups = orderItems
            .GroupBy(item => item.ModelId)
            .ToList();

        foreach (var group in modelGroups)
        {
            var modelId = group.Key;
            var totalQuantity = group.Sum(item => item.Quantity);

            try
            {
                var productModel = await _repository.GetByIdAsync(modelId, context.CancellationToken);

                if (productModel is null)
                {
                    _logger.LogWarning("ProductModel with Id {ModelId} not found", modelId);
                    continue;
                }

                // Increase overall sold count
                productModel.OverallSold += totalQuantity;

                // Update the ProductModel
                var updateResult = await _repository.UpdateAsync(modelId, productModel);

                if (updateResult.IsSuccess)
                {
                    _logger.LogInformation(
                        "Updated OverallSold for ProductModel {ModelId}: Added {Quantity}, New Total: {OverallSold}",
                        modelId, totalQuantity, productModel.OverallSold);
                }
                else
                {
                    _logger.LogError(
                        "Failed to update OverallSold for ProductModel {ModelId}: {Error}",
                        modelId, updateResult.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating OverallSold for ProductModel {ModelId}: {ErrorMessage}",
                    modelId, ex.Message);
            }
        }
    }
}
