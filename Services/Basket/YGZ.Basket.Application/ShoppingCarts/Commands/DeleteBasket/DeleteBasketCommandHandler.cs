using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.DeleteBasket;

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, bool>
{
    private readonly ILogger<DeleteBasketCommandHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository, IUserHttpContext userContext, ILogger<DeleteBasketCommandHandler> logger)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.ClearBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.ClearBasketAsync), "Failed to clear basket", new { userEmail, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully cleared basket", new { userEmail });

        return true;
    }
}
