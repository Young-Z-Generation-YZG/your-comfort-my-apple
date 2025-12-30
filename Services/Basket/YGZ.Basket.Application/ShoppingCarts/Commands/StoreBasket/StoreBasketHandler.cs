using Grpc.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Api.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly ILogger<StoreBasketHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;
    private readonly ISKUPriceCache _skuPriceCache;
    private readonly IModelSlugCache _modelSlugCache;
    private readonly IDistributedCache _distributedCache;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;

    public StoreBasketHandler(IBasketRepository basketRepository,
                              ILogger<StoreBasketHandler> logger,
                              IUserHttpContext userContext,
                              IModelSlugCache modelSlugCache,
                              IDistributedCache distributedCache,
                              ISKUPriceCache skuPriceCache,
                              CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _logger = logger;
        _skuPriceCache = skuPriceCache;
        _modelSlugCache = modelSlugCache;
        _distributedCache = distributedCache;
        _catalogProtoServiceClient = catalogProtoServiceClient;
    }


    public async Task<Result<bool>> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        string userEmail = _userContext.GetUserEmail();
        var shoppingCart = ShoppingCart.Create(_userContext.GetUserEmail(), new List<ShoppingCartItem>());

        foreach (var item in request.CartItems)
        {
            var order = 1;
            SkuModel sku;

            try
            {
                var skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                {
                    SkuId = item.SkuId
                }, cancellationToken: cancellationToken);


                // check sku
                if (skuGrpc is not null)
                {
                    sku = skuGrpc;
                    // check stock availability
                    if (skuGrpc.AvailableInStock < item.Quantity)
                    {
                        _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                            nameof(Handle), "Insufficient stock for SKU", new { skuId = item.SkuId, requestedQuantity = item.Quantity, availableStock = skuGrpc.AvailableInStock, userEmail });

                        return Errors.Basket.InsufficientQuantity;
                    }
                }
                else
                {
                    _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), "SKU not found", new { skuId = item.SkuId, userEmail });

                    return false;
                }
            }
            catch (RpcException ex)
            {
                var parameters = new { skuId = item.SkuId, quantity = item.Quantity, userEmail };
                _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), ex.Message, parameters);
                throw;
            }

            var modelVO = Model.Create(sku.NormalizedModel);
            var colorVO = Color.Create(sku.NormalizedColor);
            var storageVO = Storage.Create(sku.NormalizedStorage);
            EColor.TryFromName(sku.NormalizedColor, out var colorEnum);
            EIphoneModel.TryFromName(sku.NormalizedModel, out var modelEnum);
            EStorage.TryFromName(sku.NormalizedStorage, out var storageEnum);
            var unitPrice = await _distributedCache.GetStringAsync(CacheKeyPrefixConstants.CatalogService.GetIphoneSkuPriceKey(modelEnum: modelEnum,
                                                                                                                               colorEnum: colorEnum,
                                                                                                                               storageEnum: storageEnum));

            decimal unitPriceParsed = decimal.Parse(unitPrice ?? "0");

            var displayImageUrl = await _distributedCache.GetStringAsync(CacheKeyPrefixConstants.CatalogService.GetDisplayImageUrlKey(modelId: sku.ModelId,
                                                                                                                                      colorEnum: colorEnum));


            var cartItem = ShoppingCartItem.Create(isSelected: item.IsSelected,
                                                   modelId: sku.ModelId,
                                                   skuId: sku.Id,
                                                   productName: $"{modelVO.Name} {modelVO.Name} {modelVO.Name}",
                                                   model: modelVO,
                                                   color: colorVO,
                                                   storage: storageVO,
                                                   displayImageUrl: displayImageUrl ?? "",
                                                   unitPrice: unitPriceParsed,
                                                   promotion: null,
                                                   quantity: item.Quantity,
                                                   quantityRemain: sku.AvailableInStock ?? 0,
                                                   modelSlug: "",
                                                   order: order++);

            shoppingCart.AddCartItem(cartItem);
        }

        var result = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.StoreBasketAsync), "Failed to store basket", new { userEmail, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully stored basket", new { userEmail, cartItemCount = shoppingCart.CartItems.Count });

        return result.Response;
    }
}
