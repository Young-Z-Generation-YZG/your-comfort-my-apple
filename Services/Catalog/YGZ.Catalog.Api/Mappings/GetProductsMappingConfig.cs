using Mapster;
using YGZ.Catalog.Api.Contracts;
using YGZ.Catalog.Application.Products.Queries.GetProductsPagination;

namespace YGZ.Catalog.Api.Mappings;

public class GetProductsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<GetProductsRequest, GetProductsQuery>()
            .Map(dest => dest.Page, src => src._page)
            .Map(dest => dest.Limit, src => src._limit)
            .Map(dest => dest.ProductColor, src => src._productColor)
            .Map(dest => dest.ProductStorage, src => src._productStorage)
            .Map(dest => dest.PriceFrom, src => src._priceFrom)
            .Map(dest => dest.PriceTo, src => src._priceTo)
            .Map(dest => dest.PriceSort, src => src._priceSort);
    }
}
