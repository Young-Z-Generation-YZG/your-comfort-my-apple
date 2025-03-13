using Mapster;
using YGZ.Ordering.Api.Contracts;
using YGZ.Ordering.Application.Orders.Queries.GetOrders;

namespace YGZ.Ordering.Api.Mappings;

public class GetOrdersPaginationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<GetOrdersPaginationRequest, GetOrdersQuery>()
            .Map(dest => dest.Page, src => src._page)
            .Map(dest => dest.Limit, src => src._limit)
            .Map(dest => dest.OrderName, src => src._orderName)
            .Map(dest => dest.OrderCode, src => src._orderCode)
            .Map(dest => dest.OrderStatus, src => src._orderStatus);
    }
}
