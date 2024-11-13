
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.Products.Commands.CreateProductItem;

public class CreateProductItemCommandHandler : ICommandHandler<CreateProductItemCommand, ProductItem>
{
    public Task<Result<ProductItem>> Handle(CreateProductItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
