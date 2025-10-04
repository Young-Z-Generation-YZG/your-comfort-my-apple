
using YGZ.Catalog.Application.Reviews.Commands;
using YGZ.Catalog.Domain.Products.Iphone.Entities;

namespace YGZ.Catalog.Application.Reviews.Extensions;

public static class MappingExtensions
{
    public static Review ToEntity(this CreateReviewCommand dto, string customerId)
    {
        throw new NotImplementedException();
        //var modelId = ModelId.Of(dto.ModelId);
        //var skuId = SKUId.Of(dto.ProductId);

        //return Review.Create(modelId: modelId,
        //                     skuId: skuId,
        //                     OrderId: dto.OrderId,
        //                     orderItemId: dto.OrderItemId,
        //                     content: dto.Content,
        //                     customerFullName: dto.CustomerUserName,
        //                     customerId: customerId,
        //                     rating: dto.Rating);
    }
}
