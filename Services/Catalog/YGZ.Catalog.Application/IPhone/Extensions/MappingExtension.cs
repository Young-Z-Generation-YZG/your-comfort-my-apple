using YGZ.Catalog.Application.IPhone.Commands.CreateIphoneModel;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;

namespace YGZ.Catalog.Application.IPhone.Extensions;

public static class MappingExtension
{
    public static IphoneModel ToEntity(this CreateIphoneModelCommand dto)
    {
        List<Color> colors = dto.Colors.Select(color => Color.Create(name: color.Name,
                                                                     hexCode: color.HexCode,
                                                                     showcaseImageId: color.ShowcaseImageId,
                                                                     order: color.Order)).ToList();

        List<Image> showcaseImages = dto.ShowcaseImages.Select(image => Image.Create(imageId: image.ImageId,
                                                                                     imageUrl: image.ImageUrl,
                                                                                     imageName: image.ImageName,
                                                                                     imageDescription: image.ImageDescription,
                                                                                     imageWidth: image.ImageWidth,
                                                                                     imageHeight: image.ImageHeight,
                                                                                     imageBytes: image.ImageBytes,
                                                                                     imageOrder: image.Order)).ToList();

        List<Model> models = dto.Models.Select(model => Model.Create(name: model.Name,
                                                                     order: model.Order)).ToList();

        List<Storage> storages = dto.Storages.Select(s =>
        {
            return Storage.Create(s.Name, s.Value, s.Order);
        }).ToList();

        var ratingInit = new List<RatingStar>
        {
            RatingStar.Create(1, 0),
            RatingStar.Create(2, 0),
            RatingStar.Create(3, 0),
            RatingStar.Create(4, 0),
            RatingStar.Create(5, 0),
        };

        var categoryId = CategoryId.Of(dto.CategoryId);

        return IphoneModel.Create(ModelId.Create(),
                                  name: dto.Name,
                                  models: models,
                                  colors: colors,
                                  storages: storages,
                                  showcaseImages: showcaseImages,
                                  description: dto.Description,
                                  averageRating: AverageRating.Create(0, 0),
                                  ratingStars: ratingInit,
                                  categoryId: categoryId);
    }
}
