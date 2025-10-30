using Mapster;
using YGZ.Ordering.Api.Contracts;
using YGZ.Ordering.Application.Orders.Queries.GetOrders;

namespace YGZ.Ordering.Api.Mappings;

public class GetOrdersPaginationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<GetOrdersPaginationRequest, GetOrdersByAdminQuery>()
            .Map(dest => dest._page, src => src._page)
            .Map(dest => dest._limit, src => src._limit)
            .Map(dest => dest._customerEmail, src => src._customerName)
            .Map(dest => dest._orderCode, src => src._orderCode)
            .Map(dest => dest._orderStatus, src => src._orderStatus);
    }
}
