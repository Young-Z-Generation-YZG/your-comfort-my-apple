
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket.Extensions;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<StoreBasketCommandHandler> _logger;
    private readonly IUserContext _userContext;

    public StoreBasketCommandHandler(IBasketRepository basketRepository,
                                     ILogger<StoreBasketCommandHandler> logger,
                                     IUserContext userContext,
                                     DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _logger = logger;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();

        var discount = await _discountProtoServiceClient.GetDiscountByCodeAsync(new GetDiscountRequest { Code = "ZF8727717" });

        ShoppingCart shoppingCart = request.ToEntity("lov3rinve146@gmail.com");

        var result = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

        if(result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
