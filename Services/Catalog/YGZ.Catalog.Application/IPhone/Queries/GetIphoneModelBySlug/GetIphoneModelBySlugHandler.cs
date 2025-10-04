using MapsterMapper;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Iphone.Queries.GetIphoneModelBySlug;

public class GetIphoneModelBySlugHandler : IQueryHandler<GetIphoneModelBySlugQuery, IphoneModelDetailsResponse>
{
    private readonly IMongoRepository<SKU, SKUId> _skuRepository;
    private readonly IMongoRepository<IphoneModel, ModelId> _iphoneModelRepository;
    private readonly IMongoRepository<Branch, BranchId> _branchRepository;
    private readonly IMapper _mapper;

    public GetIphoneModelBySlugHandler(IMongoRepository<SKU, SKUId> skuRepository, IMongoRepository<IphoneModel, ModelId> iphoneModelRepository, IMongoRepository<Branch, BranchId> branchRepository, IMapper mapper)
    {
        _skuRepository = skuRepository;
        _iphoneModelRepository = iphoneModelRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<Result<IphoneModelDetailsResponse>> Handle(GetIphoneModelBySlugQuery request, CancellationToken cancellationToken)
    {

        var iphoneModel = await _iphoneModelRepository.GetByFilterAsync(Builders<IphoneModel>.Filter.Eq(x => x.Slug, Slug.Of(request.ModelSlug)), cancellationToken);

        if (iphoneModel is null)
        {
            return Errors.Iphone.ModelDoesNotExist;
        }

        var skus = await _skuRepository.GetAllAsync(Builders<SKU>.Filter.Eq(x => x.ModelId, iphoneModel.Id), cancellationToken);

        // Group SKUs by BranchId
        var skusByBranch = skus.GroupBy(sku => sku.BranchId)
                               .ToDictionary(group => group.Key, group => group.ToList());



        return await ToResponse(iphoneModel, skusByBranch, cancellationToken);
    }

    private async Task<IphoneModelDetailsResponse> ToResponse(IphoneModel model, Dictionary<BranchId, List<SKU>> skusByBranch, CancellationToken cancellationToken)
    {
        var branchsResponse = new List<BranchWithSkusResponse>();

        foreach (var (branchId, skus) in skusByBranch)
        {
            var branch = await _branchRepository.GetByIdAsync(branchId.Value!, cancellationToken)!;

            var skusResponse = skus.Select(sku => sku.ToResponse()).ToList();

            branchsResponse.Add(new BranchWithSkusResponse
            {
                Branch = branch.ToResponse(),
                Skus = skusResponse
            });
        }



        return new IphoneModelDetailsResponse
        {
            Id = model.Id.Value!,
            Name = model.Name,
            ModelItems = _mapper.Map<List<ModelResponse>>(model.Models),
            ColorItems = _mapper.Map<List<ColorResponse>>(model.Colors),
            StorageItems = _mapper.Map<List<StorageResponse>>(model.Storages),
            Description = model.Description,
            OverallSold = model.OverallSold,
            AverageRating = _mapper.Map<AverageRatingResponse>(model.AverageRating),
            CategoryId = model.CategoryId.Value!,
            Branchs = branchsResponse,
        };
    }
}
