

using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductItemRequest(string Model,
                                              string Color,
                                              int Storage,
                                              string Description,
                                              decimal Price,
                                              int Quantity_in_stock,
                                              List<ImageRequest> Images, 
                                              string ProductId,
                                              string PromotionId) { }