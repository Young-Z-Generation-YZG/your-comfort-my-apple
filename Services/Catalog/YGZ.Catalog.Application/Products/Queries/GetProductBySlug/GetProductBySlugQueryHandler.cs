

using MapsterMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Application.Products.Queries.GetProductBySlug;

public class GetProductBySlugQueryHandler : IQueryHandler<GetProductBySlugQuery, IPhoneResponse>
{
    private readonly IMongoRepository<IPhone16Detail> _detailRepository;
    private readonly ILogger<GetProductBySlugQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetProductBySlugQueryHandler(IMongoRepository<IPhone16Detail> detailRepository, ILogger<GetProductBySlugQueryHandler> logger, IMapper mapper)
    {
        _detailRepository = detailRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<IPhoneResponse>> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<IPhone16Detail>.Filter.Eq(x => x.Slug, Slug.Of(request.slug));

        var product = await _detailRepository.GetByFilterAsync(filter, cancellationToken);

        IPhoneResponse response = MapToResponse(product);

        return response;
    }

    private IPhoneResponse MapToResponse(IPhone16Detail product)
    {
        var colorResponse = _mapper.Map<ColorResponse>(product.Color);

        var imagesResponse = _mapper.Map<List<ImageResponse>>(product.Images);

        var response = _mapper.Map<IPhoneResponse>(product);

        response.ProductColor = colorResponse;

        response.ProductImages = imagesResponse;

        return response;
    }
}
