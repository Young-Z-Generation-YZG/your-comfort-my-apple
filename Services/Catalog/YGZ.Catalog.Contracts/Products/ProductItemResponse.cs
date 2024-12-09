﻿
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Domain.Core.Enums;

namespace YGZ.Catalog.Contracts.Products;

public sealed record ProductItemResponse(string Sku,
                                         string Model,
                                         string Color,
                                         int Storage,
                                         int Quantity_remain,
                                         int Quantity_in_stock,
                                         int sold,
                                         double Price,
                                         List<ImageResponse> Images,
                                         string State,
                                         DateTime Created_at,
                                         DateTime Updated_at) { }
