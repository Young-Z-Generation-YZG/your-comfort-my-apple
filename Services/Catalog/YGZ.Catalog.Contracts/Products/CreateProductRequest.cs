using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductRequest(string Name,
                                          string? Description,
                                          AverageRatingRequest? Average_rating,
                                          List<ImageRequest>? Images,
                                          List<string> Models,
                                          List<string> Colors,
                                          string? Category_id, 
                                          string? Promotion_id) { }