using Mapster;
using YGZ.Catalog.Application.Products.Commands.CreateProduct;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.Common.Mappings;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductRequest, CreateProductCommand>()
            .Ignore(dest => dest.Files);
    }
}
