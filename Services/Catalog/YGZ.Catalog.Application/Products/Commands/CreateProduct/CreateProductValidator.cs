
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using YGZ.Catalog.Application.Common.CustomValidator;


namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(product => product.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(product => product.Images)
            .NotEmpty();

        RuleFor(product => product.AverageRating)
            .NotNull()
            .Must(averageRating => averageRating.Value >= 0 && averageRating.Value <= 5)
            .Must(averageRating => averageRating.NumRatings >= 0);

        RuleFor(product => product.PromotionId)
            .Must(Validators.ObjectIdVlidator.BeAValidObjectId)
            .WithMessage("Invalid promotion id")
            .When(product => product.PromotionId != null);


        RuleFor(product => product.CategoryId)
            .Must(Validators.ObjectIdVlidator.BeAValidObjectId)
            .WithMessage("Invalid category id")
            .When(product => product.CategoryId != null);
    }
}
