using YGZ.Catalog.Application.ProductItems.Commands.CreateProductItem;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Application.ProductItems.Extensions;

public static class MappingExtension
{
    public static ProductItem ToEntity(this CreateProductItemCommand dto)
    {
        var color = Color.Create(dto.Color.ColorName,
                                 dto.Color.ColorHex,
                                 dto.Color.ColorOrder);

        var images = dto.Images.Select(image => Image.Create(image.ImageId,
                                                             image.ImageUrl,
                                                             image.ImageOrder)).ToArray();
        return ProductItem.Create(model: dto.Model,
                                  color: color,
                                  storage: dto.Storage,
                                  price: dto.Price,
                                  description: dto.Description,
                                  images: images,
                                  productId: dto.ProductId
                                  );
    }
}
