
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.DeleteBasket;

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserContext _userContext;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository, IUserContext userContext)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.DeleteBasket("lov3rinve146@gmail.com", cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
