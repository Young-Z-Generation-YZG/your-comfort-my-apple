using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserRequestContext _userContext;

    public GetBasketQueryHandler(IBasketRepository basketRepository, IUserRequestContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var shoppingCart = result.Response!;

        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            return new GetBasketResponse()
            {
                UserEmail = shoppingCart.UserEmail,
                CartItems = new List<CartItemResponse>(),
                TotalAmount = 0
            };
        }

        // Filter out event items (items with PromotionEvent) - show only regular items
        var regularItems = shoppingCart.CartItems
            .Where(ci => ci.Promotion?.PromotionEvent == null)
            .ToList();

        var cartItemResponses = regularItems.Select(item =>
        {

            return new CartItemResponse
            {
                IsSelected = item.IsSelected,
                ModelId = item.ModelId,
                ProductName = item.ProductName,
                Color = item.Color.ToResponse(),
                Model = item.Model.ToResponse(),
                Storage = item.Storage.ToResponse(),
                DisplayImageUrl = item.DisplayImageUrl,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                SubTotalAmount = item.SubTotalAmount,
                Promotion = null,
                Index = item.Order
            };
        }).ToList();

        var totalAmount = regularItems.Where(item => item.IsSelected).Sum(item => item.SubTotalAmount);

        return new GetBasketResponse()
        {
            UserEmail = shoppingCart.UserEmail,
            CartItems = cartItemResponses,
            TotalAmount = shoppingCart.TotalAmount
        };
    }
}