
using YGZ.Basket.Application.Contracts;
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;

namespace YGZ.Basket.Application.Baskets.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketByUserIdQuery, GetBasketResponse>
{
    public async Task<Result<GetBasketResponse>> Handle(GetBasketByUserIdQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine("GetBasketQueryHandler" + request.UserId);
        await Task.CompletedTask;

        throw new NotImplementedException();
    }
}
