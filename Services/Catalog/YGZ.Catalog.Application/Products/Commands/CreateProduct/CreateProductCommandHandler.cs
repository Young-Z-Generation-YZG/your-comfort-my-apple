
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, bool>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Result<bool>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _productService.CreateProductAsync(request);

        if(result.IsFailure)
        {
            return result.Error;
        }

        return result.Response;
    }
}
