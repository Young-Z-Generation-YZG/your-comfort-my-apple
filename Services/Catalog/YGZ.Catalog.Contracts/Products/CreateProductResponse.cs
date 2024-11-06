
namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductResponse(string Id,
                                           string Name,
                                           string Slug,
                                           string Description,
                                           List<ImageResponse> Images,
                                           AverageRatingResponse Average_rating,
                                           List<ProductItemResponse> Product_items,
                                           string CategoryId,
                                           string PromotionId,
                                           DateTime Created_at,
                                           DateTime Updated_at) { }

public sealed record AverageRatingResponse(double Value, int Num_ratings) { }

public sealed record ImageResponse(string Image_url, string Image_id) { }

public sealed record ProductItemResponse(string Id,
                                         string Sku,
                                         string Model,
                                         string Color,
                                         int Storage,
                                         double Price,
                                         int Quantity_in_stock,
                                          List<ImageResponse> Images,
                                         DateTime Created_at,
                                         DateTime Updated_at) { }