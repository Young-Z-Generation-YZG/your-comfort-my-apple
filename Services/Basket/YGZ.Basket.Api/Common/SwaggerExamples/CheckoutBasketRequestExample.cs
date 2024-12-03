using Swashbuckle.AspNetCore.Filters;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Api.Common.SwaggerExamples;

public class CheckoutBasketRequestExample : IExamplesProvider<CheckoutBasketRequest>
{
    public CheckoutBasketRequest GetExamples()
    {
        return new CheckoutBasketRequest("78dc45ca-a007-4d33-9616-2d8e44735e1a",
                                         "Lê Xuân Bách",
                                         "0333284890",
                                         "lov3rinve146@gmail.com",
                                         "1060 Kha Vạn Cân",
                                         "Linh Chiểu",
                                         "Thủ Đức",
                                         "Việt Nam",
                                         "VNPAY");
    }
}
