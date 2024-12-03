using Swashbuckle.AspNetCore.Filters;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Api.Common.SwaggerExamples;

public class StoreBasketRequestExample : IExamplesProvider<StoreBasketRequest>
{
    public StoreBasketRequest GetExamples()
    {
        return new StoreBasketRequest("78dc45ca-a007-4d33-9616-2d8e44735e1a", new List<CartLineRequest>
        {
            new CartLineRequest("674e8be4ec6966234e16b846", "iPhone 16", "ultramarine", 128, 799, 1),
        });
    }
}
