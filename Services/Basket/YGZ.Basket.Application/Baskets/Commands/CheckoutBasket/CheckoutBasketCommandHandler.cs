
using MassTransit;
using Microsoft.AspNetCore.Http;
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Application.Core.Abstractions.Payments;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Infrastructure.Payments.Vnpay;
using YGZ.BuildingBlocks.Messaging.ServiceEvents.BasketEvents;

namespace YGZ.Basket.Application.Baskets.Commands.CheckoutBasket;

public class CheckoutBasketCommandHandler : ICommandHandler<CheckoutBasketCommand, string>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IVnpayPaymentProvider _vnpayPaymentProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutBasketCommandHandler(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IVnpayPaymentProvider vnpayPaymentProvider, IHttpContextAccessor httpContextAccessor)
    {
        _basketRepository = basketRepository;
        _publishEndpoint = publishEndpoint;
        _vnpayPaymentProvider = vnpayPaymentProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<string>> Handle(CheckoutBasketCommand cmd, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(cmd.UserId, cancellationToken);

        if (basket.IsFailure)
        {
            return basket.Error;
        }

        var cartLines = basket.Response!.CartLines.Select(x => new OrderLineIntegrationEvent(
            x.Id.Value.ToString(),
            x.Model,
            x.Color,
            x.Storage,
            "slug",
            x.Quantity,
            x.Price
        )).ToList();

        var eventMessage = new BasketCheckoutIntegrationEvent
        {
            UserId = cmd.UserId,
            ContactName = cmd.ContactName,
            ContactPhoneNumber = cmd.ContactPhoneNumber,
            ContactEmail = cmd.ContactEmail,
            AddressLine = cmd.AddressLine,
            District = cmd.District,
            Province = cmd.Province,
            Country = cmd.Country,
            PaymentType = cmd.PaymentType,
            CartLines = cartLines
        };

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        var model = new PaymentInformationModel()
        {
            OrderType = "Order",
            OrderDescription = "Order Description",
            Amount = basket.Response.TotalAmount * 25000,
            Name = cmd.ContactName,
        };

        // Vnpay Payment
        //if (cmd.PaymentType == "VNPAY")
        //{
        //    var paymentResult = _vnpayPaymentProvider.CreatePaymentUrl(model, _httpContextAccessor.HttpContext!);

        //    return paymentResult;
        //}


        //await _basketRepository.DeleteBasket(cmd.UserId, cancellationToken);

        return "empty";
    }
}
