

using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public record CreateProductItemResponse(string Id,
                                               string Sku,
                                               string Model,
                                               string Color,
                                               int Storage,
                                               double Price,
                                               int Quantity_in_stock,
                                               List<ImageResponse> Images,
                                               DateTime Created_at,
                                               DateTime Updated_at){ }
