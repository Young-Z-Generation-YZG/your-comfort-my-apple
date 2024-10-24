using Swashbuckle.AspNetCore.Filters;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductExample : IExamplesProvider<CreateProductCommand>
{
    public CreateProductCommand GetExamples()
    {
        return new CreateProductCommand
        {
            Name = "Product Name 2123"
        };
    }
}
