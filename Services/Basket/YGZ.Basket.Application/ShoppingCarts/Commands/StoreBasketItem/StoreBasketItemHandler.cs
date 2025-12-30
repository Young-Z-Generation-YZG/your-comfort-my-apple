using Grpc.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;
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

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasketItem;

public class StoreBasketItemHandler : ICommandHandler<StoreBasketItemCommand, bool>
{
    private readonly ILogger<StoreBasketHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;
    private readonly IDistributedCache _distributedCache;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;

    public StoreBasketItemHandler(ILogger<StoreBasketHandler> logger,
                                  IBasketRepository basketRepository,
                                  IUserHttpContext userContext,
                                  CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient,
                                  IDistributedCache distributedCache)
    {
        _logger = logger;
        _basketRepository = basketRepository;
        _userContext = userContext;
        _catalogProtoServiceClient = catalogProtoServiceClient;
        _distributedCache = distributedCache;
    }

    public async Task<Result<bool>> Handle(StoreBasketItemCommand request, CancellationToken cancellationToken)
    {
        // 1. vaidate stock availability
        SkuModel sku;

        try
        {
            var skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
            {
                SkuId = request.SkuId
            }, cancellationToken: cancellationToken);


            // check sku
            if (skuGrpc is not null)
            {
                sku = skuGrpc;
                // check stock availability
                if (skuGrpc.AvailableInStock < request.Quantity)
                {
                    _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(Handle), "Insufficient stock for SKU", new { skuId = request.SkuId, requestedQuantity = request.Quantity, availableStock = skuGrpc.AvailableInStock, userEmail = _userContext.GetUserEmail() });

                    return Errors.Basket.InsufficientQuantity;
                }
            }
            else
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), "SKU not found", new { skuId = request.SkuId, userEmail = _userContext.GetUserEmail() });

                return false;
            }
        }
        catch (RpcException ex)
        {
            var parameters = new { skuId = request.SkuId, userEmail = _userContext.GetUserEmail() };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), ex.Message, parameters);
            throw;
        }


        // 2. get current shopping cart
        var shoppingCartResult = await _basketRepository.GetBasketAsync(_userContext.GetUserEmail(), cancellationToken);
        ShoppingCart shoppingCart;

        if (shoppingCartResult.Response is not null)
        {
            shoppingCart = shoppingCartResult.Response;
        }
        else
        {
            shoppingCart = ShoppingCart.Create(_userContext.GetUserEmail(), new List<ShoppingCartItem>());
        }

        // 3. Add item to cart
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


        var cartItem = ShoppingCartItem.Create(isSelected: false,
                                               modelId: sku.ModelId,
                                               skuId: sku.Id,
                                               productName: $"{modelVO.Name} {modelVO.Name} {modelVO.Name}",
                                               model: modelVO,
                                               color: colorVO,
                                               storage: storageVO,
                                               displayImageUrl: displayImageUrl ?? "",
                                               unitPrice: unitPriceParsed,
                                               promotion: null,
                                               quantity: 1,
                                               quantityRemain: sku.AvailableInStock ?? 0,
                                               modelSlug: "",
                                               order: 0);

        shoppingCart.AddCartItem(cartItem);

        var result = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.StoreBasketAsync), "Failed to store basket item", new { skuId = request.SkuId, userEmail = _userContext.GetUserEmail(), error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully stored basket item", new { skuId = request.SkuId, userEmail = _userContext.GetUserEmail(), cartItemCount = shoppingCart.CartItems.Count });

        return result.Response;
    }
}
