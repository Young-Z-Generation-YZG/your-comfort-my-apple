using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductRequest(string Name,
                                          string? Description,
                                          AverageRatingRequest? Average_rating,
                                          List<ImageRequest>? Images,
                                          string? Category_id, 
                                          string? Promotion_id) { }