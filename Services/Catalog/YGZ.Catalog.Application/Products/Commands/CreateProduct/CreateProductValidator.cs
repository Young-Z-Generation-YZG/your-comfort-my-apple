
using FluentValidation;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .MaximumLength(200);

    }
}
