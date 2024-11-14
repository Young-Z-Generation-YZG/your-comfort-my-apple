
using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record ProductItemResponse(string Sku,
                                         string Model,
                                         string Color,
                                         int Storage,
                                         int Quantity_In_stock,
                                         double Price,
                                         List<ImageResponse> images,
                                         DateTime Created_at,
                                         DateTime Updated_at) { }
