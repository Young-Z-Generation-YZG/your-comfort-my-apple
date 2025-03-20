

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16;
using MongoDB.Driver;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using MapsterMapper;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;

public class GetIPhonesByModelSlugQueryHandler : IQueryHandler<GetIPhonesByModelSlugQuery, List<ProductResponse>>
{
    private readonly IMongoRepository<IPhone16Detail> _detailRepository;
    private readonly IMongoRepository<IPhone16Model> _modelRepository;
    private readonly ILogger<GetIPhonesByModelSlugQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetIPhonesByModelSlugQueryHandler(IMongoRepository<IPhone16Detail> detailRepository, IMongoRepository<IPhone16Model> modelRepository, ILogger<GetIPhonesByModelSlugQueryHandler> logger, IMapper mapper)
    {
        _detailRepository = detailRepository;
        _modelRepository = modelRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductResponse>>> Handle(GetIPhonesByModelSlugQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<IPhone16Model>.Filter.Eq(x => x.Slug, Slug.Of(request.modelSlug));

        var model = await _modelRepository.GetByFilterAsync(filter, cancellationToken);

        if(model is null)
        {
            return Errors.Category.DoesNotExist;
        }

        var products = await _detailRepository.GetAllAsync(Builders<IPhone16Detail>.Filter.Eq(x => x.IPhoneModelId, model.Id), cancellationToken);

        List<ProductResponse> responses = MapToResponse(products);

        return responses;
    }

    private List<ProductResponse> MapToResponse(List<IPhone16Detail> products)
    {

        var response = products.Select(product =>
        {
            var colorResponse = _mapper.Map<ColorResponse>(product.Color);
            var imagesResponse = _mapper.Map<List<ImageResponse>>(product.Images);
            var response = _mapper.Map<ProductResponse>(product);
            response.ProductColor = colorResponse;
            response.ProductImages = imagesResponse;

            return response;
        }).ToList();

        return response;
    }
}
