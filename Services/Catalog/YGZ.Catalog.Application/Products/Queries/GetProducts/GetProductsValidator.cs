

using FluentValidation;
using YGZ.Catalog.Application.Products.Queries.GetProductsPagination;

namespace YGZ.Catalog.Application.Products.Queries.GetProducts;

public class GetProductsValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Limit).GreaterThanOrEqualTo(1);
    }
}
