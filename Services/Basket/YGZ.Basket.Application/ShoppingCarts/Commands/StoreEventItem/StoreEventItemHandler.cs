using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Cache.Entities;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
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
    private readonly IUserRequestContext _userContext;
    private readonly ISKUPriceCache _skuPriceCache;
    private readonly IColorImageCache _colorImageCache;
    private readonly IModelSlugCache _modelSlugCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<StoreEventItemHandler> _logger;

    public StoreEventItemHandler(
        IBasketRepository basketRepository,
        IUserRequestContext userContext,
        DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
        ISKUPriceCache skuPriceCache,
        IColorImageCache colorImageCache,
        IModelSlugCache modelSlugCache,
        IDistributedCache distributedCache,
        ILogger<StoreEventItemHandler> logger)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
        _skuPriceCache = skuPriceCache;
        _colorImageCache = colorImageCache;
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
                _logger.LogError("Event item with ID {EventItemId} not found", request.EventItemId);

                throw new Exception("Event item not found");
            }

            _logger.LogInformation("Retrieved event item: {Model} {Storage} {Color} - Price: {Price}",
                eventItem.Model.Name, eventItem.Storage.Name, eventItem.Color.Name, eventItem.OriginalPrice);

            // Get existing basket
            var result = await _basketRepository.GetBasketAsync(_userContext.GetUserEmail(), cancellationToken);

            if (result.IsFailure)
            {
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

            var storeResult = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

            if (storeResult.IsFailure)
            {
                return storeResult.Error;
            }

            _logger.LogInformation("Successfully added event item {EventItemId} to basket", request.EventItemId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event item with ID {EventItemId}", request.EventItemId);
            throw;
        }
    }

    private async Task<ShoppingCartItem> ShoppingCartItemMapping(EventItemModel eventItem)
    {
        var model = Model.Create(eventItem.Model.NormalizedName);
        var color = Color.Create(eventItem.Color.NormalizedName);
        var storage = Storage.Create(eventItem.Storage.NormalizedName);

        var skuPriceCacheKey = CacheKeyPrefixConstants.CatalogService.GetIphoneSkuPriceKey(EIphoneModel.FromName(eventItem.Model.NormalizedName),
                                                                                        EStorage.FromName(eventItem.Storage.NormalizedName),
                                                                                        EColor.FromName(eventItem.Color.NormalizedName));
        var modelSlugCache = ModelSlugCache.Of("");

        var unitPrice = await _distributedCache.GetStringAsync(skuPriceCacheKey);
        var modelSlug = await _modelSlugCache.GetSlugAsync(modelSlugCache);

        var displayImageUrl = eventItem.DisplayImageUrl ?? string.Empty;

        decimal originalPrice = decimal.Parse(unitPrice ?? "0");
        decimal discountValue = (decimal)(eventItem.DiscountValue ?? 0);

        BuildingBlocks.Shared.Enums.EDiscountType discountTypeEnum = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(eventItem.DiscountType.ToString());

        PromotionEvent promotionEvent = PromotionEvent.Create(eventId: "",
                                                              eventItemId: eventItem.Id,
                                                              productUnitPrice: originalPrice,
                                                              discountType: discountTypeEnum,
                                                              discountValue: discountValue);

        Promotion promotion = Promotion.Create(
            promotionType: EPromotionType.EVENT.Name,
            PromotionCoupon: null,
            PromotionEvent: promotionEvent
        );

        decimal finalPrice;
        if (eventItem.DiscountType == EDiscountTypeGrpc.DiscountTypePercentage)
        {
            finalPrice = originalPrice * (1 - discountValue);
        }
        else
        {
            finalPrice = originalPrice - discountValue;
        }

        var quantity = 1; // Default quantity for event items

        var shoppingCartItem = ShoppingCartItem.Create(
            isSelected: true,
            modelId: eventItem.Id,
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

        return shoppingCartItem;
    }
}
