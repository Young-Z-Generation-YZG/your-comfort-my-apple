

using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Domain.Core.Enums;

namespace YGZ.Catalog.Contracts.Products;

public sealed record ProductResponse(string Id,
                                    string CategoryId,
                                    string PromotionId,
                                    string Name,
                                    string Description,
                                    List<ModelResponse> Models,
                                    List<ColorResponse> Colors,
                                    List<StorageResponse> Storages,
                                    AverageRatingResponse Average_rating,
                                    List<StarRatingResponse> Star_ratings,
                                    List<ProductItemResponse> Product_items,
                                    List<ImageResponse> Images,
                                    string Slug,
                                    string State,
                                    DateTime Created_at,
                                    DateTime Updated_at) { }

public sealed record ModelResponse(string Name, int Order) { }

