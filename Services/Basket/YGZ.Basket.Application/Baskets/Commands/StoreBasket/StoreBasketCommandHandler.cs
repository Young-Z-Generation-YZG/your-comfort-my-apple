

using GYZ.Discount.Grpc;
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Catalog.Api.Protos;

namespace YGZ.Basket.Application.Baskets.Commands.StoreBasket;

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;

    public StoreBasketCommandHandler(IBasketRepository basketRepository,
                                     DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
                                     CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _discountProtoServiceClient = discountProtoServiceClient;
        _catalogProtoServiceClient = catalogProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        if(Guid.TryParse(request.UserId, out Guid userId) == false)
        {
            return Errors.Guid.IdInvalid;
        }

        var coupon = await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest
        {
            Code = request.CouponCode
        },
        cancellationToken: cancellationToken
        );

        var cartLines = new List<CartLine>();

        foreach (var productItem in request.CartLines)
        {
            try
            {
                var item = await _catalogProtoServiceClient.GetProductItemByIdAsync(new GetProductItemByIdRequest
                {
                    ProductItemId = productItem.ProductItemId
                },
                cancellationToken: cancellationToken
                );

                cartLines.Add(CartLine.CreateNew(
                item.Id,
                item.Model,
                item.Color,
                item.Storage,
                item.PrimaryImageUrl,
                productItem.Quantity,
                item.Price
                ));
            }
            catch
            {
                return Errors.Basket.InvalidCartLine(productItem.ProductItemId, productItem.Model, productItem.Color, productItem.Storage);
            }
        }

        var shoppingCart = ShoppingCart.CreateNew(
            Guid.Parse(request.UserId),
            cartLines
        );

        await _basketRepository.StoreBasket(shoppingCart, cancellationToken);

        return true;
    }
}
