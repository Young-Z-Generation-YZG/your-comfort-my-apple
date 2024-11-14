
using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductResponse(string Id,
                                           string Name,
                                           string Slug,
                                           string Description,
                                           List<ImageResponse> Images,
                                           AverageRatingResponse Average_rating,
                                           //List<ProductItemResponse> Product_items,
                                           List<string> Models,
                                           List<string> Colors,
                                           string CategoryId,
                                           string PromotionId,
                                           DateTime Created_at,
                                           DateTime Updated_at) { }

//public sealed record ProductItemResponse : CreateProductItemResponse
//{
//    ProductItemResponse(CreateProductItemResponse original) : base(original) { }
//}