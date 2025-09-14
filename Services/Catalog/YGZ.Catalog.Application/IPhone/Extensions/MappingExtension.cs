using YGZ.Catalog.Application.IPhone.Commands.CreateIphoneModel;
using YGZ.Catalog.Application.IPhone16.Commands.CreateIPhone16Detail;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Application.IPhone.Extensions;

public static class MappingExtension
{
    //public static IPhone16Model ToEntity(this CreateIPhone16ModelCommand dto)
    //{
    //    List<ColorBK> colors = dto.Colors.Select(color => ColorBK.Create(color.ColorName,
    //                                                                 color.ColorHex,
    //                                                                 color.ColorImage,
    //                                                                 color.ColorOrder)).ToList();

    //    var descriptionImages = dto.DescriptionImages.Select(image => Image.Create(imageId: image.ImageId,
    //                                                                            imageUrl: image.ImageUrl,
    //                                                                            imageName: image.ImageName,
    //                                                                            imageDescription: image.ImageDescription,
    //                                                                            imageWidth: image.ImageWidth,
    //                                                                            imageHeight: image.ImageHeight,
    //                                                                            imageBytes: image.ImageBytes,
    //                                                                            imageOrder: image.ImageOrder)).ToList();

    //    var models = dto.Models.Select(model => ModelBK.Create(modelName: model.ModelName,
    //                                                         modelOrder: model.ModelOrder)).ToList();

    //    var storages = dto.Storages.Select(s => Storage.FromValue(s)).ToList();

    //    var ratingInit = new List<RatingStar>
    //    {
    //        RatingStar.Create(1, 0),
    //        RatingStar.Create(2, 0),
    //        RatingStar.Create(3, 0),
    //        RatingStar.Create(4, 0),
    //        RatingStar.Create(5, 0),
    //    };

    //    var categoryId = string.IsNullOrEmpty(dto.CategoryId) ? null : CategoryId.Of(dto.CategoryId);

    //    return IPhone16Model.Create(
    //                                IPhone16ModelId.Create(),
    //                                name: dto.Name,
    //                                models: models,
    //                                colors: colors,
    //                                storages: storages,
    //                                description: dto.Description,
    //                                averageRating: AverageRating.Create(0, 0),
    //                                ratingStars: ratingInit,
    //                                descriptionImages: descriptionImages,
    //                                categoryId: categoryId);
    //}

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
            return Storage.Create(s.Name, s.Value);
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

        return IphoneModel.Create(
                                    ModelId.Create(),
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

    public static IPhone16Detail ToEntity(this CreateIPhone16DetailsCommand dto)
    {
        var color = ColorBK.Create(dto.Color.ColorName,
                                 dto.Color.ColorHex,
                                 dto.Color.ColorImage,
                                 dto.Color.ColorOrder);

        var images = dto.Images.Select(image => Image.Create(imageId: image.ImageId,
                                                             imageUrl: image.ImageUrl,
                                                             imageName: image.ImageName,
                                                             imageDescription: image.ImageDescription,
                                                             imageWidth: image.ImageWidth,
                                                             imageHeight: image.ImageHeight,
                                                             imageBytes: image.ImageBytes,
                                                             imageOrder: image.Order)).ToList();
        return IPhone16Detail.Create(model: dto.Model,
                                  color: color,
                                  storage: dto.Storage,
                                  unitPrice: dto.UnitPrice,
                                  description: dto.Description,
                                  images: images,
                                  iPhoneModelId: dto.IPhoneModelId,
                                  categoryId: dto.CategoryId
                                  );
    }
}
