
using MassTransit;
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.BuildingBlocks.Messaging.ServiceEvents.BasketEvents;

namespace YGZ.Basket.Application.Baskets.Commands.CheckoutBasket;

public class CheckoutBasketCommandHandler : ICommandHandler<CheckoutBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;


    public CheckoutBasketCommandHandler(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint)
    {
        _basketRepository = basketRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<bool>> Handle(CheckoutBasketCommand cmd, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(cmd.UserId, cancellationToken);

        if (basket.IsFailure)
        {
            return basket.Error;
        }

        var eventMessage = new BasketCheckoutIntergrationEvent
        {
            UserId = cmd.UserId,
            FirstName = cmd.FirstName,
            LastName = cmd.LastName,
            TotalPrice = cmd.TotalPrice,
        };

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        await _basketRepository.DeleteBasket(cmd.UserId, cancellationToken);

        return true;
    }
}
