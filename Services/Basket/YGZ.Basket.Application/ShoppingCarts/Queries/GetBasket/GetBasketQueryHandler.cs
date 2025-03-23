
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserContext _userContext;

    public GetBasketQueryHandler(IBasketRepository basketRepository, IUserContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();

        if(request.CouponCode is not null)
        {
            var discount = await _discountProtoServiceClient.GetDiscountByCodeAsync(new GetDiscountRequest { Code = request.CouponCode });

            if(discount is not null)
            {

            }
        }


        var result = await _basketRepository.GetBasketAsync("lov3rinve146@gmail.com", cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var response = new GetBasketResponse
        {
            UserEmail = result.Response!.UserEmail,
            CartItems = result.Response!.Items.Select(x => new CartItemResponse(x.ProductId,
                                                                                x.ProductModel,
                                                                                x.ProductColor,
                                                                                x.ProductStorage,
                                                                                x.ProductUnitPrice,
                                                                                x.ProductImage,
                                                                                x.Quantity)).ToList()
        };

        return response;
    }
}