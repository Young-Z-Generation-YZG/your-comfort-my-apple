using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductRequest(string Name,
                                          string? Description,
                                          List<ImageRequest>? Images,
                                          List<ModelRequest> Models,
                                          List<ColorRequest> Colors,
                                          List<int> Storages,
                                          string? CategoryId, 
                                          string? PromotionId) { }

public sealed record ModelRequest(string Name, int Order) { }
public sealed record ColorRequest(string Name, string ColorHash, string ImageColorUrl, int Order) { }