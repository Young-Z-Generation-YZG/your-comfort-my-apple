using FluentValidation;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.Common.SwaggerExamples.Producs;

public class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples()
    {
        return new CreateProductRequest("iPhone 16", "iPhone 16 description", new(0, 0), new() { new("image_url", "image_id") }, "672cdaed4e67692dff64a47c", "672cdaed4e67692dff64a47c");
    }
}
