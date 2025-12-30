using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Cache.Entities;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreEventItem;

public class StoreEventItemHandler : ICommandHandler<StoreEventItemCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserHttpContext _userContext;
    private readonly IModelSlugCache _modelSlugCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<StoreEventItemHandler> _logger;

    public StoreEventItemHandler(IBasketRepository basketRepository,
                                 IUserHttpContext userContext,
                                 DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
                                 IModelSlugCache modelSlugCache,
                                 IDistributedCache distributedCache,
                                 ILogger<StoreEventItemHandler> logger)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
        _modelSlugCache = modelSlugCache;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(StoreEventItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get event item from Discount service via gRPC
            var grpcRequest = new GetEventItemByIdRequest
            {
                EventItemId = request.EventItemId
            };

            var eventItem = await _discountProtoServiceClient.GetEventItemByIdGrpcAsync(grpcRequest, cancellationToken: cancellationToken);

            if (eventItem == null || string.IsNullOrEmpty(eventItem.Id))
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), "Event item not found", new { eventItemId = request.EventItemId, userEmail = _userContext.GetUserEmail() });

                throw new Exception("Event item not found");
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), "Retrieved event item", new { eventItemId = request.EventItemId, model = eventItem.Model.Name, storage = eventItem.Storage.Name, color = eventItem.Color.Name, price = eventItem.OriginalPrice, userEmail = _userContext.GetUserEmail() });

            // Get existing basket
            var result = await _basketRepository.GetBasketAsync(_userContext.GetUserEmail(), cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_basketRepository.GetBasketAsync), "Failed to retrieve basket from repository", new { eventItemId = request.EventItemId, userEmail = _userContext.GetUserEmail(), error = result.Error });

                return result.Error;
            }

            ShoppingCart shoppingCart = result.Response!;

            var eventItems = shoppingCart.CartItems.Where(ci => ci.Promotion?.PromotionEvent != null).ToList();

            foreach (var item in eventItems)
            {
                shoppingCart.CartItems.Remove(item);
            }

            // Create and add new cart item
            ShoppingCartItem cartItem = await ShoppingCartItemMapping(eventItem);
            shoppingCart.CartItems.Add(cartItem);

            shoppingCart.PromotionId = cartItem.Promotion?.PromotionEvent.PromotionId;
            shoppingCart.DiscountType = cartItem.Promotion?.PromotionEvent.DiscountType;
            shoppingCart.DiscountAmount = cartItem.DiscountAmount;
            shoppingCart.DiscountValue = cartItem.Promotion?.PromotionEvent.DiscountValue;
            shoppingCart.MaxDiscountAmount = null;

            var storeResult = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

            if (storeResult.IsFailure)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_basketRepository.StoreBasketAsync), "Failed to store event item in basket", new { eventItemId = request.EventItemId, userEmail = _userContext.GetUserEmail(), error = storeResult.Error });

                return storeResult.Error;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully added event item to basket", new { eventItemId = request.EventItemId, userEmail = _userContext.GetUserEmail(), cartItemCount = shoppingCart.CartItems.Count });
            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { eventItemId = request.EventItemId, userEmail = _userContext.GetUserEmail() };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            throw;
        }
    }

    private async Task<ShoppingCartItem> ShoppingCartItemMapping(EventItemModel eventItem)
    {
        var model = Model.Create(eventItem.Model.NormalizedName);
        var color = Color.Create(eventItem.Color.NormalizedName);
        var storage = Storage.Create(eventItem.Storage.NormalizedName);

        var skuPriceCacheKey = CacheKeyPrefixConstants.CatalogService.GetIphoneSkuPriceKey(modelEnum: EIphoneModel.FromName(eventItem.Model.NormalizedName),
                                                                                           colorEnum: EColor.FromName(eventItem.Color.NormalizedName),
                                                                                           storageEnum: EStorage.FromName(eventItem.Storage.NormalizedName));
        var modelSlugCache = ModelSlugCache.Of("");

        var unitPrice = await _distributedCache.GetStringAsync(skuPriceCacheKey);
        var modelSlug = await _modelSlugCache.GetSlugAsync(modelSlugCache);

        var displayImageUrl = eventItem.DisplayImageUrl ?? string.Empty;

        decimal originalPrice = decimal.Parse(unitPrice ?? "0");
        decimal discountValue = (decimal)(eventItem.DiscountValue ?? 0);

        BuildingBlocks.Shared.Enums.EDiscountType discountTypeEnum = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(eventItem.DiscountType.ToString());

        PromotionEvent promotionEvent = PromotionEvent.Create(promotionId: eventItem.EventId,
                                                              promotionType: EPromotionType.EVENT_ITEM.Name,
                                                              discountType: discountTypeEnum,
                                                              discountValue: discountValue);

        Promotion promotion = Promotion.Create(
            promotionType: EPromotionType.EVENT_ITEM.Name,
            PromotionCoupon: null,
            PromotionEvent: promotionEvent
        );



        var quantity = 1;


        var shoppingCartItem = ShoppingCartItem.Create(
            isSelected: true,
            modelId: "missing model id",
            skuId: eventItem.SkuId,
            productName: $"{eventItem.Model.Name} {eventItem.Storage.Name} {eventItem.Color.Name}",
            model: model,
            color: color,
            storage: storage,
            displayImageUrl: displayImageUrl,
            unitPrice: originalPrice,
            promotion: promotion,
            quantity: quantity,
            modelSlug: modelSlug ?? string.Empty,
            order: 1
        );

        shoppingCartItem.DiscountAmount = eventItem.DiscountAmount.HasValue ? (decimal)eventItem.DiscountAmount.Value : 0;

        return shoppingCartItem;
    }
}
