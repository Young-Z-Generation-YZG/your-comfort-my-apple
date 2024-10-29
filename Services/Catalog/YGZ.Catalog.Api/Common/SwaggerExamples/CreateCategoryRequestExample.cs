using FluentValidation;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Contracts.Categories;

namespace YGZ.Catalog.Api.Common.SwaggerExamples;

public class CreateCategoryRequestExample : AbstractValidator<CreateCategoryRequest>, IExamplesProvider<CreateCategoryRequest>
{
    public CreateCategoryRequest GetExamples()
    {
        return new CreateCategoryRequest(Name: "Category name", Description: "Category description", ParentId: "0");
    }

    public CreateCategoryRequestExample()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
