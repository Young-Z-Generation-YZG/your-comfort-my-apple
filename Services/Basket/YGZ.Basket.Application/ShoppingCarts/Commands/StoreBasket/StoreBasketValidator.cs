

using FluentValidation;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketValidator()
    {
        RuleFor(x => x.Cart)
            .NotNull()
            .NotEmpty()
            .WithMessage("Cart cannot be null or empty");
    }
}
