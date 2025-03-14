

using MassTransit;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket.Extensions;
using YGZ.Basket.Domain.Core.Enums;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

public sealed record CheckoutBasketCommandHandler : ICommandHandler<CheckoutBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishIntegrationEvent;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserContext _userContext;

    public CheckoutBasketCommandHandler(IBasketRepository basketRepository,
                                        IPublishEndpoint publishEndpoint,
                                        IUserContext userContext,
                                        DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _publishIntegrationEvent = publishEndpoint;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();
        //var userId = _userContext.GetUserId();
        decimal discountAmount = 0;
        decimal subTotal = 0;
        decimal total = 0;

        var basket = await _basketRepository.GetBasketAsync("lov3rinve146@gmail.com", cancellationToken);

        if (basket.Response == null || !basket.Response.Items.Any())
        {
            return false;
        }


        if(!string.IsNullOrEmpty(request.DiscountCode) && basket.Response!.Items.Any())
        {
            var discount = await _discountProtoServiceClient.GetDiscountByCodeAsync(new GetDiscountRequest { Code = request.DiscountCode });

            if(discount is null)
            {
                return false;
            }

            var discountType = DiscountTypeEnum.FromValue((int)discount.Coupon.Type);

            switch (discountType)
            {
                case var _ when discountType == DiscountTypeEnum.PERCENT:
                    discountAmount = basket.Response.Items.Sum(x => x.ProductPrice * x.Quantity) * (decimal)discount.Coupon.DiscountValue / 100;
                    break;
                case var _ when discountType == DiscountTypeEnum.FIXED:
                    discountAmount = basket.Response.Items.Sum(x => x.ProductPrice * x.Quantity) - (decimal)discount.Coupon.DiscountValue;
                    break;
            }
        }

        subTotal = basket.Response.Items.Sum(x => x.ProductPrice * x.Quantity);
        total = subTotal - discountAmount;

        if ((discountAmount != request.DiscountAmount) || (subTotal != request.SubTotal || (total != request.Total)))
        {
            return false;
        }

        var eventMessage = request.ToBasketCheckoutIntegrationEvent("id-test",
                                                                    "lov3rinve146@gmail.com",
                                                                    basket.Response.Items,
                                                                    discountAmount,
                                                                    subTotal,
                                                                    total);

        await _publishIntegrationEvent.Publish(eventMessage, cancellationToken);

        return true;
    }
}
