
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Inventory.Commands.UpdateSkuCommand;

public class DeductQuantityHandler : ICommandHandler<DeductQuantityCommand, bool>
{
    private readonly ILogger<DeductQuantityHandler> _logger;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;

    public DeductQuantityHandler(ILogger<DeductQuantityHandler> logger, IMongoRepository<SKU, SkuId> skuRepository, IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _skuRepository = skuRepository;
        _productModelRepository = productModelRepository;
    }

    public async Task<Result<bool>> Handle(DeductQuantityCommand request, CancellationToken cancellationToken)
    {
        foreach (var orderItem in request.Order.OrderItems)
        {
            var sku = await _skuRepository.GetByIdAsync(orderItem.SkuId, cancellationToken);

            if (sku is null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_skuRepository.GetByIdAsync), "SKU not found", new { skuId = orderItem.SkuId, orderId = request.Order.OrderId });

                return Errors.Inventory.SkuDoesNotExist;
            }

            if (sku.AvailableInStock < orderItem.Quantity)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Insufficient stock", new { skuId = orderItem.SkuId, availableStock = sku.AvailableInStock, requestedQuantity = orderItem.Quantity, orderId = request.Order.OrderId });

                return Errors.Inventory.QuantityIsLessThanTheQuantityToDeduct;
            }


            if (orderItem.PromotionType == EPromotionType.EVENT_ITEM.Name)
            {
                sku.DeductReservedQuantity(orderItem.Quantity);
            }
            else
            {
                sku.DeductQuantity(orderItem.Quantity);
            }

            // increase sold quantity in product model
            var productModel = await _productModelRepository.GetByIdAsync(sku.ModelId.Value!, cancellationToken);

            if (productModel is not null)
            {
                productModel.IncreaseSoldQuantity(orderItem.Quantity);
                await _productModelRepository.UpdateAsync(productModel.Id.Value!, productModel);
            }

            var updateSkuResult = await _skuRepository.UpdateAsync(sku.Id.Value!, sku);

            if (updateSkuResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_skuRepository.UpdateAsync), "Failed to update SKU after deducting quantity", new { skuId = orderItem.SkuId, quantity = orderItem.Quantity, orderId = request.Order.OrderId, error = updateSkuResult.Error });

                return updateSkuResult.Error;
            }

            if (productModel is not null)
            {
                var updateModelResult = await _productModelRepository.UpdateAsync(productModel.Id.Value!, productModel);

                if (updateModelResult.IsFailure)
                {
                    _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_productModelRepository.UpdateAsync), "Failed to update product model sold quantity", new { modelId = sku.ModelId.Value, skuId = orderItem.SkuId, quantity = orderItem.Quantity, error = updateModelResult.Error });

                    return updateModelResult.Error;
                }
            }
        }

        _logger.LogInformation("::[Handler Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully deducted quantities for order", new { orderId = request.Order.OrderId, itemCount = request.Order.OrderItems.Count });

        // var orderItems = request.Order.OrderItems;

        // // Group order items by ModelId to aggregate quantities per ProductModel
        // var modelGroups = orderItems
        //     .GroupBy(item => item.ModelId)
        //     .ToList();

        // foreach (var group in modelGroups)
        // {
        //     var modelId = group.Key;
        //     var totalQuantity = group.Sum(item => item.Quantity);

        //     try
        //     {
        //         var productModel = await _productModelRepository.GetByIdAsync(modelId, cancellationToken);

        //         if (productModel is null)
        //         {
        //             _logger.LogWarning("ProductModel with Id {ModelId} not found", modelId);
        //             continue;
        //         }

        //         // Increase overall sold count
        //         productModel.OverallSold += totalQuantity;

        //         // Update the ProductModel
        //         var updateResult = await _productModelRepository.UpdateAsync(modelId, productModel);

        //         if (updateResult.IsSuccess)
        //         {
        //             _logger.LogInformation(
        //                 "Updated OverallSold for ProductModel {ModelId}: Added {Quantity}, New Total: {OverallSold}",
        //                 modelId, totalQuantity, productModel.OverallSold);
        //         }
        //         else
        //         {
        //             _logger.LogError(
        //                 "Failed to update OverallSold for ProductModel {ModelId}: {Error}",
        //                 modelId, updateResult.Error);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex,
        //             "Error updating OverallSold for ProductModel {ModelId}: {ErrorMessage}",
        //             modelId, ex.Message);
        //     }
        // }

        return true;
    }
}
