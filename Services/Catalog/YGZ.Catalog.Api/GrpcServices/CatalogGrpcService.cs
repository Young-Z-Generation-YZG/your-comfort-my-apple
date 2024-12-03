using Grpc.Core;
using MediatR;
using YGZ.Catalog.Api.Protos;
using YGZ.Catalog.Application.ProductItems.Queries.GetProductItemById;

namespace YGZ.Catalog.Api.Services;

public class CatalogGrpcService : CatalogProtoService.CatalogProtoServiceBase
{
    private readonly ISender _mediator;

    public CatalogGrpcService(ISender mediator)
    {
        _mediator = mediator;
    }

    public override async Task<Product> GetProductItemById(GetProductItemByIdRequest request, ServerCallContext context)
    {
        var query = new GetProductItemByIdQuery(request.ProductItemId);

        var result = await _mediator.Send(query);

        if(result.IsFailure)
        {
            throw new RpcException(new Status(StatusCode.NotFound, result.Error.Message));
        }

        var product = result.Response;

        var reponse = new Product
        {
            Id = product!.Id,
            Sku = product.Sku,
            Model = product.Model,
            Color = product.Color,
            Storage = product.Storage,
            Price = (double?)product.Price,
            Description = product.Description,
            PrimaryImageUrl = product.Images.FirstOrDefault()!.Url,
        };

        return reponse;
    }
}
