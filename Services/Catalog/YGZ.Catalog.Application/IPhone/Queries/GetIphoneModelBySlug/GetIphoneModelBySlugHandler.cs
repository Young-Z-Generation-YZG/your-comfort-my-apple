using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Iphone.Queries.GetIphoneModelBySlug;

public class GetIphoneModelBySlugHandler : IQueryHandler<GetIphoneModelBySlugQuery, IphoneModelDetailsResponse>
{
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly IMongoRepository<Branch, BranchId> _branchRepository;

    public GetIphoneModelBySlugHandler(IMongoRepository<SKU, SkuId> skuRepository, IMongoRepository<ProductModel, ModelId> productModelRepository, IMongoRepository<Branch, BranchId> branchRepository)
    {
        _skuRepository = skuRepository;
        _productModelRepository = productModelRepository;
        _branchRepository = branchRepository;
    }

    public async Task<Result<IphoneModelDetailsResponse>> Handle(GetIphoneModelBySlugQuery request, CancellationToken cancellationToken)
    {

        var iphoneModel = await _productModelRepository.GetByFilterAsync(Builders<ProductModel>.Filter.Eq(x => x.Slug, Slug.Of(request.ModelSlug)), cancellationToken);

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

    private async Task<IphoneModelDetailsResponse> ToResponse(ProductModel model, Dictionary<BranchId, List<SKU>> skusByBranch, CancellationToken cancellationToken)
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
            Category = model.Category.ToResponse(),
            Name = model.Name,
            ModelItems = model.Models.Select(x => x.ToResponse()).ToList(),
            ColorItems = model.Colors.Select(x => x.ToResponse()).ToList(),
            StorageItems = model.Storages.Select(x => x.ToResponse()).ToList(),
            SkuPrices = model.Prices.Select(x => x.ToResponse()).ToList(),
            Description = model.Description,
            ShowcaseImages = model.ShowcaseImages.Select(x => x.ToResponse()).ToList(),
            OverallSold = model.OverallSold,
            RatingStars = model.RatingStars.Select(x => x.ToResponse()).ToList(),
            AverageRating = model.AverageRating.ToResponse(),
            Branchs = branchsResponse,
        };
    }
}
