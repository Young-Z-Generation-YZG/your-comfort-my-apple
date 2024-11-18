

using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Application.Baskets.Commands.StoreBasket;

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;

    public StoreBasketCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
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

        var shoppingCart = ShoppingCart.CreateNew(
            Guid.Parse(request.UserId),
            request.CartLines.ConvertAll(line => CartLine.CreateNew(
                line.ProductId,
                line.Sku,
                line.Model,
                line.Color,
                line.Storage,
                line.Quantity,
                line.Price,
                line.ImageUrl))
        );

        await _basketRepository.StoreBasket(shoppingCart, cancellationToken);

        return true;
    }
}
