
using FluentValidation;
using MongoDB.Bson;
using YGZ.Catalog.Application.Common.CustomValidator;

namespace YGZ.Catalog.Application.Products.Commands.CreateProductItem;

public class CreateProductItemValidator : AbstractValidator<CreateProductItemCommand>
{
    public CreateProductItemValidator()
    {
        RuleFor(product => product.Model)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(product => product.Color)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(product => product.Storage)
            .GreaterThan(0);

        RuleFor(product => product.Price)
            .GreaterThan(0);

        RuleFor(product => product.Quantity_in_stock)
            .GreaterThan(0);

        RuleFor(product => product.ProductId)
            .NotEmpty()
            .Must(Validators.ObjectIdVlidator.BeAValidObjectId).WithMessage("Invalid product id");
    }
}
