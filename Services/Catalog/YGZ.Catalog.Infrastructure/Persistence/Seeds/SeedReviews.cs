using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public static class SeedReviews
{
    public static IEnumerable<Review> Reviews
    {
        get
        {
            var customerReviewInfo1 = CustomerReviewInfo.Create(name: "Bach Le", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo2 = CustomerReviewInfo.Create(name: "Tran Danh", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo3 = CustomerReviewInfo.Create(name: "Dang Duong", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo4 = CustomerReviewInfo.Create(name: "Kong Le", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo5 = CustomerReviewInfo.Create(name: "An Nguyen", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo6 = CustomerReviewInfo.Create(name: "Khang Vo", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo7 = CustomerReviewInfo.Create(name: "Trang Quang Khai", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo8 = CustomerReviewInfo.Create(name: "Bao Du", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo9 = CustomerReviewInfo.Create(name: "Nhat Tan", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo10 = CustomerReviewInfo.Create(name: "Tien Le", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo11 = CustomerReviewInfo.Create(name: "Cong Chinh", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo12 = CustomerReviewInfo.Create(name: "Quy Nguyen", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo13 = CustomerReviewInfo.Create(name: "Khanh Nguyen", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo14 = CustomerReviewInfo.Create(name: "Minh Minh", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo15 = CustomerReviewInfo.Create(name: "Huy Hoang", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo16 = CustomerReviewInfo.Create(name: "Hieu Nguyen", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo17 = CustomerReviewInfo.Create(name: "Danh Nguyen", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo18 = CustomerReviewInfo.Create(name: "Duc Pham", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo19 = CustomerReviewInfo.Create(name: "Hai Dang", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");
            var customerReviewInfo20 = CustomerReviewInfo.Create(name: "Long Nguyen", avatarImageUrl: "https://picsum.photos/300/300/?blur=1");

            var iphone15ModelId = ModelId.Of("664351e90087aa09993f5ae7");
            var iphone16ModelId = ModelId.Of("6643543d0087aa09993f5b14");
            var iphone16eModelId = ModelId.Of("6643543e0087aa09993f5b15");
            var iphone17ModelId = ModelId.Of("6643543f0087aa09993f5b16");
            var iphone17ProModelId = ModelId.Of("664354400087aa09993f5b17");
            var iphone17AirModelId = ModelId.Of("664354410087aa09993f5b18");

            var skuId1 = SkuId.Of("690f4601e2295b9f94f23f5f"); // IPHONE-IPHONE_15-BLUE-128GB
            var skuId2 = SkuId.Of("690f4601e2295b9f94f23f60"); // IPHONE-IPHONE_15-BLUE-256GB
            var skuId3 = SkuId.Of("690f4601e2295b9f94f23f61"); // IPHONE-IPHONE_15-BLUE-512GB
            var skuId4 = SkuId.Of("690f4601e2295b9f94f23f62"); // IPHONE-IPHONE_15-BLUE-1TB
            var skuId5 = SkuId.Of("690f4601e2295b9f94f23f63"); // IPHONE-IPHONE_15-PINK-128GB

            var skuId6 = SkuId.Of("690f4601e2295b9f94f23f87"); // IPHONE-IPHONE_16-ULTRAMARINE-128GB
            var skuId7 = SkuId.Of("690f4601e2295b9f94f23f88"); // IPHONE-IPHONE_16-ULTRAMARINE-256GB
            var skuId8 = SkuId.Of("690f4601e2295b9f94f23f89"); // IPHONE-IPHONE_16-ULTRAMARINE-512GB
            var skuId9 = SkuId.Of("690f4601e2295b9f94f23f8a"); // IPHONE-IPHONE_16-ULTRAMARINE-1TB
            var skuId10 = SkuId.Of("690f4601e2295b9f94f23f8b"); // IPHONE-IPHONE_16-TEAL-128GB

            var skuId11 = SkuId.Of("690f4601e2295b9f94f23fb0"); // IPHONE-IPHONE_16E-WHITE-128GB
            var skuId12 = SkuId.Of("690f4601e2295b9f94f23fb1"); // IPHONE-IPHONE_16E-WHITE-256GB
            var skuId13 = SkuId.Of("690f4601e2295b9f94f23fb2"); // IPHONE-IPHONE_16E-WHITE-512GB
            var skuId14 = SkuId.Of("690f4601e2295b9f94f23fb3"); // IPHONE-IPHONE_16E-WHITE-1TB
            var skuId15 = SkuId.Of("690f4601e2295b9f94f23fb4"); // IPHONE-IPHONE_16E-BLACK-128GB

            var skuId16 = SkuId.Of("690f4601e2295b9f94f23fb8"); // IPHONE-IPHONE_17-LAVENDER-128GB
            var skuId17 = SkuId.Of("690f4601e2295b9f94f23fb9"); // IPHONE-IPHONE_17-LAVENDER-256GB
            var skuId18 = SkuId.Of("690f4601e2295b9f94f23fba"); // IPHONE-IPHONE_15-BLUE-512GB
            var skuId19 = SkuId.Of("690f4601e2295b9f94f23fbb"); // IPHONE-IPHONE_15-BLUE-1TB
            var skuId20 = SkuId.Of("690f4601e2295b9f94f23fbc"); // IPHONE-IPHONE_17-SAGE-128GB

            var skuId21 = SkuId.Of("690f4601e2295b9f94f23fcc"); // IPHONE-IPHONE_15-BLUE-128GB
            var skuId22 = SkuId.Of("690f4601e2295b9f94f23fcd"); // IPHONE-IPHONE_15-BLUE-256GB
            var skuId23 = SkuId.Of("690f4601e2295b9f94f23fce"); // IPHONE-IPHONE_15-BLUE-512GB
            var skuId24 = SkuId.Of("690f4601e2295b9f94f23fcf"); // IPHONE-IPHONE_17_PRO-SILVER-1TB
            var skuId25 = SkuId.Of("690f4601e2295b9f94f23fd0"); // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-128GB

            var skuId26 = SkuId.Of("690f4601e2295b9f94f23fe4"); // IPHONE-IPHONE_17_AIR-SKY_BLUE-128GB
            var skuId27 = SkuId.Of("690f4601e2295b9f94f23fe5"); // IPHONE-IPHONE_17_AIR-SKY_BLUE-256GB
            var skuId28 = SkuId.Of("690f4601e2295b9f94f23fe6"); // IPHONE-IPHONE_17_AIR-SKY_BLUE-512GB
            var skuId29 = SkuId.Of("690f4601e2295b9f94f23fe7"); // IPHONE-IPHONE_17_AIR-SKY_BLUE-1TB
            var skuId30 = SkuId.Of("690f4601e2295b9f94f23fe8"); // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-128GB







            // Reviews for iPhone 15
            var review1 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId1,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo1,
                content: "Excellent phone! The camera quality is outstanding and the battery life is impressive.",
                rating: 5
            );

            var review2 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId2,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo2,
                content: "Great value for money. The performance is smooth and the design is beautiful.",
                rating: 4
            );

            var review3 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId3,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo3,
                content: "Amazing product! Very satisfied with my purchase. Highly recommend!",
                rating: 5
            );

            var review4 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId4,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo4,
                content: "Good phone overall, but the storage could be better. Still happy with it.",
                rating: 4
            );

            var review5 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId5,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo5,
                content: "Perfect phone for daily use. The display is crisp and colors are vibrant.",
                rating: 5
            );

            var review6 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId1,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo6,
                content: "Decent phone but nothing extraordinary. It works well for basic tasks.",
                rating: 3
            );

            var review7 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId2,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo7,
                content: "Love the build quality and user experience. Worth every penny!",
                rating: 5
            );

            var review8 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId3,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo8,
                content: "Good phone with solid performance. The camera could be better though.",
                rating: 4
            );

            var review9 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId4,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo9,
                content: "Excellent device! Fast, reliable, and the design is modern.",
                rating: 5
            );

            var review10 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone15ModelId,
                skuId: skuId5,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo10,
                content: "Average phone. It works but I expected more features for the price.",
                rating: 3
            );







            // Reviews for iPhone 16
            var review11 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId6,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo1,
                content: "Outstanding phone! The new features are amazing and the performance is top-notch.",
                rating: 5
            );

            var review12 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId7,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo2,
                content: "Great upgrade from previous model. The display and camera are significantly better.",
                rating: 5
            );

            var review13 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId8,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo3,
                content: "Very satisfied with this purchase. The battery life is excellent!",
                rating: 4
            );

            var review14 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId9,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo4,
                content: "Good phone but the price is a bit high. Still worth it for the quality.",
                rating: 4
            );

            var review15 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId10,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo5,
                content: "Amazing device! The processing speed is incredible and apps run smoothly.",
                rating: 5
            );

            var review16 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId6,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo6,
                content: "Decent phone. The design is nice but I expected more innovation.",
                rating: 3
            );

            var review17 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId7,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo7,
                content: "Perfect phone for photography enthusiasts. The camera quality is superb!",
                rating: 5
            );

            var review18 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId8,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo8,
                content: "Good overall experience. The user interface is intuitive and responsive.",
                rating: 4
            );

            var review19 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId9,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo9,
                content: "Excellent build quality and premium feel. Very happy with this purchase!",
                rating: 5
            );

            var review20 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16ModelId,
                skuId: skuId10,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo10,
                content: "Average phone. It works fine but nothing special stands out.",
                rating: 3
            );



            // Reviews for iPhone 16e
            var review21 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId11,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo11,
                content: "Great budget-friendly option! Good performance for the price point.",
                rating: 4
            );

            var review22 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId12,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo12,
                content: "Excellent value! This phone offers great features at an affordable price.",
                rating: 5
            );

            var review23 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId13,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo13,
                content: "Good phone for everyday use. The battery lasts long and performance is solid.",
                rating: 4
            );

            var review24 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId14,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo14,
                content: "Satisfied with the purchase. The design is clean and modern.",
                rating: 4
            );

            var review25 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId15,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo15,
                content: "Amazing phone for the price! Highly recommend to anyone looking for value.",
                rating: 5
            );

            var review26 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId11,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo16,
                content: "Decent phone but some features are missing compared to premium models.",
                rating: 3
            );

            var review27 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId12,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo17,
                content: "Good performance and reliable. Perfect for students and professionals.",
                rating: 4
            );

            var review28 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId13,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo18,
                content: "Excellent phone! The camera quality is impressive for this price range.",
                rating: 5
            );

            var review29 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId14,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo19,
                content: "Solid phone with good build quality. Very happy with my purchase!",
                rating: 4
            );

            var review30 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone16eModelId,
                skuId: skuId15,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo20,
                content: "Average phone. It works but I expected better performance.",
                rating: 3
            );








            // Reviews for iPhone 17
            var review31 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId16,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo1,
                content: "Outstanding phone! The latest features are incredible and performance is top-tier.",
                rating: 5
            );

            var review32 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId17,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo2,
                content: "Amazing upgrade! The new processor makes everything run so smoothly.",
                rating: 5
            );

            var review33 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId18,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo3,
                content: "Excellent phone with premium features. The display quality is outstanding!",
                rating: 5
            );

            var review34 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId19,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo4,
                content: "Great phone but the price is quite high. Still worth it for the quality.",
                rating: 4
            );

            var review35 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId20,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo5,
                content: "Perfect device! The camera and battery life exceed all expectations.",
                rating: 5
            );

            var review36 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId16,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo6,
                content: "Good phone overall. Some features could be improved but it works well.",
                rating: 4
            );

            var review37 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId17,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo7,
                content: "Incredible performance! This phone handles everything I throw at it.",
                rating: 5
            );

            var review38 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId18,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo8,
                content: "Satisfied with the purchase. The design is elegant and modern.",
                rating: 4
            );

            var review39 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId19,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo9,
                content: "Excellent build quality! This phone feels premium and performs excellently.",
                rating: 5
            );

            var review40 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ModelId,
                skuId: skuId20,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo10,
                content: "Decent phone but expected more innovation for a flagship model.",
                rating: 3
            );



            // Reviews for iPhone 17 Pro
            var review41 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId21,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo11,
                content: "Premium phone with exceptional features! The Pro camera system is incredible.",
                rating: 5
            );

            var review42 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId22,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo12,
                content: "Best phone I've ever owned! The performance and camera quality are unmatched.",
                rating: 5
            );

            var review43 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId23,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo13,
                content: "Outstanding device! The Pro features make a huge difference in daily use.",
                rating: 5
            );

            var review44 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId24,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo14,
                content: "Excellent phone but very expensive. Worth it if you need the Pro features.",
                rating: 4
            );

            var review45 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId25,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo15,
                content: "Perfect for professionals! The advanced features are exactly what I needed.",
                rating: 5
            );

            var review46 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId21,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo16,
                content: "Great phone with premium build. The display is stunning and colors are vibrant.",
                rating: 5
            );

            var review47 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId22,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo17,
                content: "Excellent performance! This phone handles multitasking like a champ.",
                rating: 5
            );

            var review48 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId23,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo18,
                content: "Good phone overall. The Pro features are nice but not essential for everyone.",
                rating: 4
            );

            var review49 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId24,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo19,
                content: "Amazing device! The camera system is professional-grade and photos are stunning.",
                rating: 5
            );

            var review50 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17ProModelId,
                skuId: skuId25,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo20,
                content: "Premium phone with great features. The price is high but quality is top-notch.",
                rating: 4
            );






            // Reviews for iPhone 17 Air
            var review51 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId26,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo1,
                content: "Lightweight and powerful! Perfect balance of performance and portability.",
                rating: 5
            );

            var review52 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId27,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo2,
                content: "Excellent phone! The Air design is sleek and the performance is impressive.",
                rating: 5
            );

            var review53 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId28,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo3,
                content: "Great phone for those who want premium features in a lighter package.",
                rating: 4
            );

            var review54 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId29,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo4,
                content: "Love the lightweight design! The phone feels great in hand and performs well.",
                rating: 5
            );

            var review55 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId30,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo5,
                content: "Perfect combination of style and performance. Very satisfied with this purchase!",
                rating: 5
            );

            var review56 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId26,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo6,
                content: "Good phone but I expected more features for the Air model.",
                rating: 3
            );

            var review57 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId27,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo7,
                content: "Excellent device! The lightweight design doesn't compromise on performance.",
                rating: 5
            );

            var review58 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId28,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo8,
                content: "Great phone with modern design. The battery life is impressive!",
                rating: 4
            );

            var review59 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId29,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo9,
                content: "Amazing phone! The Air model offers great features in a portable package.",
                rating: 5
            );

            var review60 = Review.Create(
                reviewId: ReviewId.Create(),
                modelId: iphone17AirModelId,
                skuId: skuId30,
                orderInfo: OrderInfo.Create(orderId: "SEED_DATA", orderItemId: "SEED_DATA"),
                customerReviewInfo: customerReviewInfo10,
                content: "Decent phone. The lightweight design is nice but performance could be better.",
                rating: 3
            );




            return new List<Review>
            {
                review1,
                review2,
                review3,
                review4,
                review5,
                review6,
                review7,
                review8,
                review9,
                review10,
                review11,
                review12,
                review13,
                review14,
                review15,
                review16,
                review17,
                review18,
                review19,
                review20,
                review21,
                review22,
                review23,
                review24,
                review25,
                review26,
                review27,
                review28,
                review29,
                review30,
                review31,
                review32,
                review33,
                review34,
                review35,
                review36,
                review37,
                review38,
                review39,
                review40,
                review41,
                review42,
                review43,
                review44,
                review45,
                review46,
                review47,
                review48,
                review49,
                review50,
                review51,
                review52,
                review53,
                review54,
                review55,
                review56,
                review57,
                review58,
                review59,
                review60
            };
        }
    }
}
