
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;

namespace YGZ.Basket.Application.Baskets.Commands.DeleteBasket;

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Result<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _basketRepository.DeleteBasket(request.UserId, cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
