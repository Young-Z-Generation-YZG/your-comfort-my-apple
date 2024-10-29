
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Application.Core.Abstractions.Products;
using YGZ.Catalog.Application.Core.Abstractions.Uploading;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, bool>
{
    private readonly IProductService _productService;
    private readonly IUploadService _uploadSerivce;

    public CreateProductCommandHandler(IProductService productService, IUploadService uploadService)
    {
        _productService = productService;
        _uploadSerivce = uploadService;
    }

    public async Task<Result<bool>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {

        //var uploadResult = await _uploadSerivce.UploadImageFileAsync(request.File);

        var result = await _productService.CreateProductAsync(request);

        if(result.IsFailure)
        {
            return result.Error;
        }

        return result.Response;
    }
}
