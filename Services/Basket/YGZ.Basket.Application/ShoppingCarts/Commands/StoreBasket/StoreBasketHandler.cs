using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<StoreBasketHandler> _logger;
    private readonly IUserRequestContext _userContext;

    public StoreBasketHandler(IBasketRepository basketRepository,
                              ILogger<StoreBasketHandler> logger,
                              IUserRequestContext userContext,
                              DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
        _logger = logger;
    }


    public async Task<Result<bool>> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        string userEmail = _userContext.GetUserEmail();
        
        List<ShoppingCartItem> cartItems = ShoppingCartItemMapping(request.CartItems);
        ShoppingCart shoppingCart = ShoppingCart.Create(userEmail, cartItems);

        var result = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Response;
    }

    private List<ShoppingCartItem> ShoppingCartItemMapping(List<CartItemCommand> cartItems)
    {
        var shoppingCartItems = new List<ShoppingCartItem>();
        var order = 1;

        foreach (var item in cartItems)
        {
            var model = Model.Create(item.Model.Name);
            var color = Color.Create(item.Color.Name);
            var storage = Storage.Create(item.Storage.Name);

            Promotion? promotion = null;
            if (item.Promotion != null)
            {
                PromotionCoupon? promotionCoupon = null;
                PromotionEvent? promotionEvent = null;

                if (item.Promotion.PromotionCoupon != null)
                {
                    promotionCoupon = PromotionCoupon.Create(item.Promotion.PromotionCoupon.CouponId);
                }

                if (item.Promotion.PromotionEvent != null)
                {
                    promotionEvent = PromotionEvent.Create(
                        item.Promotion.PromotionEvent.EventId,
                        item.Promotion.PromotionEvent.EventItemId
                    );
                }

                promotion = Promotion.Create(
                    item.Promotion.PromotionType,
                    promotionCoupon,
                    promotionEvent
                );
            }

            var subTotalAmount = item.UnitPrice * item.Quantity;

            var shoppingCartItem = ShoppingCartItem.Create(
                modelId: item.ModelId,
                productName: item.ProductName,
                model: model,
                color: color,
                storage: storage,
                displayImageUrl: item.DisplayImageUrl,
                unitPrice: item.UnitPrice,
                promotion: promotion,
                quantity: item.Quantity,
                subTotalAmount: subTotalAmount,
                modelSlug: item.ModelSlug,
                order: order++
            );

            shoppingCartItems.Add(shoppingCartItem);
        }

        return shoppingCartItems;
    }
}
