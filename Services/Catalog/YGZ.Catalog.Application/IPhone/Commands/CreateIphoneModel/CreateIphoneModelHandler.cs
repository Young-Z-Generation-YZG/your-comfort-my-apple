
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Application.IPhone.Commands.CreateIphoneModel;

public class CreateIPhoneModelCommandHandler : ICommandHandler<CreateIphoneModelCommand, bool>
{
    private readonly IMongoRepository<IphoneModel, ModelId> _iphoneModelRepository;
    private readonly IMongoRepository<Category, CategoryId> _categoryRepository;


    public CreateIPhoneModelCommandHandler(IMongoRepository<IphoneModel, ModelId> iphoneModelRepository, IMongoRepository<Category, CategoryId> categoryRepository)
    {
        _iphoneModelRepository = iphoneModelRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<bool>> Handle(CreateIphoneModelCommand request, CancellationToken cancellationToken)
    {
        Category? category = null;

        if (request.CategoryId is not null)
        {
            category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        }

        List<Model> models = request.Models.Select(model => Model.Create(model.Name, model.Order)).ToList();
        List<Color> colors = request.Colors.Select(color => Color.Create(color.Name, color.HexCode, color.ShowcaseImageId, color.Order)).ToList();
        List<Storage> storages = request.Storages.Select(storage => Storage.Create(storage.Name, storage.Value, storage.Order)).ToList();
        List<Image> showcaseImages = request.ShowcaseImages.Select(image => Image.Create(image.ImageId, image.ImageUrl, image.ImageName, image.ImageDescription, image.ImageWidth, image.ImageHeight, image.ImageBytes, image.Order)).ToList();
        AverageRating initAverageRating = AverageRating.Create(0, 0);
        List<RatingStar> initRatingStars = new List<RatingStar>
        {
            RatingStar.Create(1, 0),
            RatingStar.Create(2, 0),
            RatingStar.Create(3, 0),
            RatingStar.Create(4, 0),
            RatingStar.Create(5, 0),
        };

        List<IphoneSkuPriceList> prices = new List<IphoneSkuPriceList>();
        List<Model> modelsInPrices = new()
            {
                Model.Create("IPHONE_15", 0),
                Model.Create("IPHONE_15_PLUS", 0)
            };

        List<Color> colorsInPrices = new()
            {
                Color.Create("BLUE", "#000000", "#000000", 0),
                Color.Create("PINK", "#000000", "#000000", 0),
                Color.Create("YELLOW", "#000000", "#000000", 0),
                Color.Create("GREEN", "#000000", "#000000", 0),
                Color.Create("BLACK", "#000000", "#000000", 0),
            };

        List<Storage> storagesInPrices = new()
            {
                Storage.Create("128GB", 128, 0),
                Storage.Create("256GB", 256, 0),
                Storage.Create("512GB", 512, 0),
                Storage.Create("1TB", 1024, 0),
            };
        // init
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[0].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[0].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[1].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[2].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[3].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[0].NormalizedName, 1000));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[1].NormalizedName, 1100));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[2].NormalizedName, 1200));
        prices.Add(IphoneSkuPriceList.Create(modelsInPrices[1].NormalizedName, colorsInPrices[4].NormalizedName, storagesInPrices[3].NormalizedName, 1300));

        var newModel = IphoneModel.Create(iPhoneModelId: ModelId.Create(),
                                          category: category,
                                          name: request.Name,
                                          models: models,
                                          colors: colors,
                                          storages: storages,
                                          prices: prices,
                                          showcaseImages: showcaseImages,
                                          description: request.Description,
                                          averageRating: initAverageRating,
                                          ratingStars: initRatingStars);

        var result = await _iphoneModelRepository.InsertOneAsync(newModel);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Response;
    }
}
