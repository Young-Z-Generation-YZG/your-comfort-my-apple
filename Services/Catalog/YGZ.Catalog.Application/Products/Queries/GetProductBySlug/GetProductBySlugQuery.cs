﻿

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;

namespace YGZ.Catalog.Application.Products.Queries.GetProductBySlug;

public sealed record GetProductBySlugQuery(string slug) : IQuery<IPhoneDetailsWithPromotionResponse>;