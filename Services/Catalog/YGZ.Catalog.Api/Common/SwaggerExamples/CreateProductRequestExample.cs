using FluentValidation;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.Common.SwaggerExamples;

public class CreateProductRequestExample : AbstractValidator<CreateProductRequest>, IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples()
    {
        return new CreateProductRequest(Name: "Test product", Files: []);
    }

    public CreateProductRequestExample()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
