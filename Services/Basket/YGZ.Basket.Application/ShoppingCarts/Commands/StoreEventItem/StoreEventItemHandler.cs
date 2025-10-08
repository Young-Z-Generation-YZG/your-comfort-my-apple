using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreEventItem;

public class StoreEventItemHandler : ICommandHandler<StoreEventItemCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserRequestContext _userContext;

    public StoreEventItemHandler(IBasketRepository basketRepository,
                                 IUserRequestContext userContext,
                                 DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public Task<Result<bool>> Handle(StoreEventItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
