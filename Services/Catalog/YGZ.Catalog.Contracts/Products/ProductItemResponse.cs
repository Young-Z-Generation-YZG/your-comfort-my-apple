﻿
using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record ProductItemResponse(string Id,
                                         string Sku,
                                         string Model,
                                         string Color,
                                         int Storage,
                                         string Description,
                                         int Quantity_remain,
                                         int Quantity_in_stock,
                                         int Sold,
                                         decimal Price,
                                         List<ImageResponse> Images,
                                         string State,
                                         DateTime Created_at,
                                         DateTime Updated_at) { }
