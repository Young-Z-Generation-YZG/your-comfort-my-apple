
using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record ProductItemResponse(string Sku,
                                         string Model,
                                         string Color,
                                         int Storage,
                                         string Description,
                                         int Quantity_remain,
                                         int Quantity_in_stock,
                                         int Sold,
                                         double Price,
                                         List<ImageResponse> Images,
                                         string State,
                                         DateTime Created_at,
                                         DateTime Updated_at) { }
