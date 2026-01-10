using Mapster;
using YGZ.Catalog.Api.Contracts.SkuRequestRequest;
using YGZ.Catalog.Application.Requests.Queries.GetSkuRequests;

namespace YGZ.Catalog.Api.Mappings;

public class SkuRequestMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetSkuRequestsRequest, GetSkuRequestsQuery>()
            .Map(dest => dest.Page, src => src._page)
            .Map(dest => dest.Limit, src => src._limit);
    }
}
