using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;


public static class SeedCategoryData
{
    public static IEnumerable<Category> Categories
    {
        get
        {
            return new List<Category>
            {
                Category.Create(id: CategoryId.Of("67dc43ee9b19c6773e9cec55"), name: "Mac", description: "Mac categories.", order: 0, parentId: null),
                Category.Create(id: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa"), name: "iPad", description: "iPad categories.", order: 1, parentId: null),
                Category.Create(id: CategoryId.Of("67dc470aa9ee0a5e6fbafdab"), name: "iPhone", description: "iPhone categories.", order: 2, parentId: null),
                Category.Create(id: CategoryId.Of("67dc470ca9ee0a5e6fbafdac"), name: "Watch", description: "Watch categories.", order: 3, parentId: null),
                Category.Create(id: CategoryId.Of("67dc470ea9ee0a5e6fbafdad"), name: "HeadPhones", description: "HeadPhones categories.", order: 4, parentId: null),

                Category.Create(id: CategoryId.Of("67dc43ee9b19c6773e9cec41"), name: "MacBook Air", description: "MacBookAir category.", order: 0, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
                Category.Create(id: CategoryId.Of("68dc43ee9b19c6773e9cec42"), name: "MacBook Pro", description: "MacBook Pro category.", order: 1, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
                Category.Create(id: CategoryId.Of("69dc43ee9b19c6773e9cec43"), name: "iMac", description: "iMac category.", order: 2, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
                Category.Create(id: CategoryId.Of("70dc43ee9b19c6773e9cec44"), name: "Mac mini", description: "Mac mini category.", order: 3, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
                Category.Create(id: CategoryId.Of("71dc43ee9b19c6773e9cec45"), name: "Mac Studio", description: "Mac Studio category.", order: 4, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
                Category.Create(id: CategoryId.Of("72dc43ee9b19c6773e9cec46"), name: "Mac Pro", description: "Mac Pro category.", order: 5, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),
                Category.Create(id: CategoryId.Of("73dc43ee9b19c6773e9cec47"), name: "Displays", description: "Displays category.", order: 6, parentId: CategoryId.Of("67dc43ee9b19c6773e9cec55")),

                Category.Create(id: CategoryId.Of("81c4708a9ee0a5e6fbafdabd"), name: "iPad Pro", description: "iPad Pro category.", order: 0, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
                Category.Create(id: CategoryId.Of("82dc4708a9ee0a5e6fbafdac"), name: "iPad Air", description: "iPad Air category.", order: 1, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
                Category.Create(id: CategoryId.Of("67dc5336a9ee0a5e6fbafdb3"), name: "iPad", description: "iPad category.", order: 2, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
                Category.Create(id: CategoryId.Of("67dc5338a9ee0a5e6fbafdb4"), name: "iPad mini", description: "iPad mini category.", order: 3, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
                Category.Create(id: CategoryId.Of("67dc533aa9ee0a5e6fbafdb5"), name: "Apple Pencil", description: "Apple Pencil category.", order: 4, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),
                Category.Create(id: CategoryId.Of("67dc533ca9ee0a5e6fbafdb6"), name: "Keyboards", description: "Keyboards category.", order: 5, parentId: CategoryId.Of("67dc4708a9ee0a5e6fbafdaa")),

                Category.Create(id: CategoryId.Of("91dc470aa9ee0a5e6fbafdbc"), name: "iPhone 16", description: "iPhone 16 category.", order: 0, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
                Category.Create(id: CategoryId.Of("92dc470aa9ee0a5e6fbafdbd"), name: "iPhone 15", description: "iPhone 15 category.", order: 1, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
                Category.Create(id: CategoryId.Of("93dc470aa9ee0a5e6fbafdbe"), name: "iPhone 14", description: "iPhone 14 category.", order: 2, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
                Category.Create(id: CategoryId.Of("94dc470aa9ee0a5e6fbafdbf"), name: "iPhone 13", description: "iPhone 13 category.", order: 3, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),
                Category.Create(id: CategoryId.Of("95dc470aa9ee0a5e6fbafdbg"), name: "iPhone 12", description: "iPhone 12 category.", order: 4, parentId: CategoryId.Of("67dc470aa9ee0a5e6fbafdab")),

                Category.Create(id: CategoryId.Of("67dc5378a9ee0a5e6fbafdb8"), name: "Apple Watch Series 10", description: "Apple Watch Series 10 category.", order: 0, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),
                Category.Create(id: CategoryId.Of("67dc5379a9ee0a5e6fbafdb9"), name: "Apple Watch Series 9", description: "Apple Watch Series 9 category.", order: 1, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),
                Category.Create(id: CategoryId.Of("67dc537ba9ee0a5e6fbafdba"), name: "Apple Watch Ultra 2", description: "Apple Watch Ultra 2 category.", order: 2, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),
                Category.Create(id: CategoryId.Of("67dc537ca9ee0a5e6fbafdbb"), name: "Apple Watch SE 2", description: "Apple Watch SE 2 category.", order: 3, parentId: CategoryId.Of("67dc470ca9ee0a5e6fbafdac")),

                Category.Create(id: CategoryId.Of("67dc5397a9ee0a5e6fbafdbc"), name: "AirPods", description: "AirPods category.", order: 1, parentId: CategoryId.Of("67dc470ea9ee0a5e6fbafdad")),
                Category.Create(id: CategoryId.Of("67dc539aa9ee0a5e6fbafdbd"), name: "EarPods", description: "EarPods category.", order: 2, parentId: CategoryId.Of("67dc470ea9ee0a5e6fbafdad")),
            };

        }
    }
}
