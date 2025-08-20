
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.DeleteBasket;

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserRequestContext _userContext;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository, IUserRequestContext userContext)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.DeleteBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
