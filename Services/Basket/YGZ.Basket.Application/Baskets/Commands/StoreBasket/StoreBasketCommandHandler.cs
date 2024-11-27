

using GYZ.Discount.Grpc;
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Application.Baskets.Commands.StoreBasket;

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public StoreBasketCommandHandler(IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Guid.Parse(request.UserId);
        }
        catch
        {
            return Errors.Guid.IdInvalid;
        }

        //var coupon = await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest
        //{
        //    ProductName = "IPhone X"
        //}, 
        //cancellationToken: cancellationToken
        //);



        var shoppingCart = ShoppingCart.CreateNew(
            Guid.Parse(request.UserId),
            request.CartLines.ConvertAll(line => CartLine.CreateNew(
                line.ProductItemId,
                line.Model,
                line.Color,
                line.Storage,
                line.Quantity,
                line.Price))
        );

        await _basketRepository.StoreBasket(shoppingCart, cancellationToken);

        return true;
    }
}
