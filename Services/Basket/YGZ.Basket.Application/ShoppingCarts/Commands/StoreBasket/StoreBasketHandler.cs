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

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly ILogger<StoreBasketHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IUserRequestContext _userContext;
    private readonly ISKUPriceCache _skuPriceCache;
    private readonly IModelSlugCache _modelSlugCache;
    private readonly IDistributedCache _distributedCache;

    public StoreBasketHandler(IBasketRepository basketRepository,
                              ILogger<StoreBasketHandler> logger,
                              IUserRequestContext userContext,
                              IModelSlugCache modelSlugCache,
                              IDistributedCache distributedCache,
                              ISKUPriceCache skuPriceCache)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _logger = logger;
        _skuPriceCache = skuPriceCache;
        _modelSlugCache = modelSlugCache;
        _distributedCache = distributedCache;
    }


    public async Task<Result<bool>> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        string userEmail = _userContext.GetUserEmail();

        List<ShoppingCartItem> cartItems = await ShoppingCartItemMapping(request.CartItems);
        ShoppingCart shoppingCart = ShoppingCart.Create(userEmail, cartItems);

        var result = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Response;
    }

    private async Task<List<ShoppingCartItem>> ShoppingCartItemMapping(List<CartItemCommand> cartItems)
    {
        var shoppingCartItems = new List<ShoppingCartItem>();
        var order = 1;

        foreach (var item in cartItems)
        {
            var model = Model.Create(item.Model.Name);
            var color = Color.Create(item.Color.Name);
            var storage = Storage.Create(item.Storage.Name);
            var skuPriceCache = PriceCache.Of(model, color, storage);
            var colorImageCache = ColorImageCache.Of(item.ModelId, color);
            var modelSlugCache = ModelSlugCache.Of(item.ModelId);

            EColor.TryFromName(color.NormalizedName, out var colorEnum);

            var unitPrice = await _skuPriceCache.GetPriceAsync(skuPriceCache);
            var displayImageUrl = await _distributedCache.GetStringAsync(CacheKeyPrefixConstants.CatalogService.GetDisplayImageUrlKey(item.ModelId, colorEnum));
            var modelSlug = await _modelSlugCache.GetSlugAsync(modelSlugCache);


            var subTotalAmount = unitPrice * item.Quantity;

            var shoppingCartItem = ShoppingCartItem.Create(
                isSelected: item.IsSelected,
                modelId: item.ModelId,
                productName: $"{model.Name} {storage.Name} {color.Name}",
                model: model,
                color: color,
                storage: storage,
                displayImageUrl: displayImageUrl ?? "",
                unitPrice: unitPrice ?? 0,
                promotion: null,
                quantity: item.Quantity,
                modelSlug: modelSlug ?? "",
                order: order++
            );

            shoppingCartItems.Add(shoppingCartItem);
        }

        return shoppingCartItems;
    }
}
