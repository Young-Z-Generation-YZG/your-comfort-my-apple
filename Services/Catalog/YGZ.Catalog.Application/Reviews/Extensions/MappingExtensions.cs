
using YGZ.Catalog.Application.Reviews.Commands;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Application.Reviews.Extensions;

public static class MappingExtensions
{
    public static Review ToEntity(this CreateReviewCommand dto, string customerId)
    {
        var productId = IPhone16Id.Of(dto.ProductId);
        var modelId = IPhone16ModelId.Of(dto.ModelId);

        return Review.Create(content: dto.Content,
                             rating: dto.Rating,
                             productId: productId,
                             modelId: modelId,
                             orderItemId: dto.OrderItemId,
                             customerId: customerId);
    }
}
