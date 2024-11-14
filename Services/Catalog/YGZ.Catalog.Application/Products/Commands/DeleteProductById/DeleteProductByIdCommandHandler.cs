
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.Products.Commands.DeleteProductById;

public class DeleteProductByIdCommandHandler : ICommandHandler<DeleteProductByIdCommand, bool>
{
    private readonly IProductService _productSerivce;

    public DeleteProductByIdCommandHandler(IProductService productSerivce)
    {
        _productSerivce = productSerivce;
    }

    public async Task<Result<bool>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _productSerivce.FindByIdAsync(request.Id, cancellationToken);

        if(product is null)
        {
            return Errors.Product.DoesNotExist;
        }

        var result = await _productSerivce.DeleteOneAsync(request.Id, null!, cancellationToken);

        return result;
    }
}
