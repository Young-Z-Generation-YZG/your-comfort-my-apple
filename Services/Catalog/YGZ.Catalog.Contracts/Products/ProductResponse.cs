﻿

using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record ProductResponse(string Id,
                                    string Name,
                                    string Description,
                                    List<string> Models,
                                    List<string> Colors,
                                    AverageRatingResponse Average_rating,
                                    List<ProductItemResponse> Product_items,
                                    List<ImageResponse> Images,
                                    string Slug,
                                    string CategoryId,
                                    string PromotionId,
                                    DateTime Created_at,
                                    DateTime Updated_at) { }
