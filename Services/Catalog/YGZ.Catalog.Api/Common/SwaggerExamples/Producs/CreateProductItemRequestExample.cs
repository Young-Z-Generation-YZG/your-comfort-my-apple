using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.Common.SwaggerExamples.Producs;

public class CreateProductItemRequestExample : IExamplesProvider<CreateProductItemRequest>
{
    public CreateProductItemRequest GetExamples()
    {
        return new CreateProductItemRequest("iPhone 16", "pink", 256, 1200, 5, new() { new("image_url", "iamge_id", 1)}, "67346f7549189f7314e4ef0c");
    }
}
