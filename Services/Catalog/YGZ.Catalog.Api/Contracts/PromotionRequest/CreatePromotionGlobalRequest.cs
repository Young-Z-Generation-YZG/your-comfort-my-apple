using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public sealed record CreatePromotionGlobalRequest
{
    [Required]
    [JsonPropertyName("global_title")]
    required public string GlobalTitle { get; set; }

    [Required]
    [JsonPropertyName("global_description")]
    required public string GlobalDescription { get; set; }

    [Required]
    [JsonPropertyName("global_type")]
    required public string GlobalType { get; set; }

    [Required]
    [JsonPropertyName("event_id")]
    required public string EventId { get; set; }

    [JsonPropertyName("promotion_categories")]
    public List<PromotionCategoryRequest> PromotionCategories { get; set; } = new();

    [JsonPropertyName("promotion_products")]
    public List<PromotionProductRequest> PromotionProducts { get; set; } = new();
}

public sealed record PromotionCategoryRequest
{
    [Required]
    [JsonPropertyName("category_id")]
    required public string CategoryId { get; set; }

    [Required]
    [JsonPropertyName("category_name")]
    required public string CategoryName { get; set; }

    [Required]
    [JsonPropertyName("category_slug")]
    required public string CategorySlug { get; set; }

    [Required]
    [JsonPropertyName("discount_type")]
    required public string DiscountType { get; set; }

    [Required]
    [JsonPropertyName("discount_value")]
    required public decimal DiscountValue { get; set; }
}

public sealed record PromotionProductRequest
{
    [Required]
    [JsonPropertyName("product_id")]
    required public string ProductId { get; set; }

    [Required]
    [JsonPropertyName("product_slug")]
    required public string ProductSlug { get; set; }

    [Required]
    [JsonPropertyName("product_image")]
    required public string ProductImage { get; set; }

    [Required]
    [JsonPropertyName("discount_type")]
    required public string DiscountType { get; set; }

    [Required]
    [JsonPropertyName("discount_value")]
    required public decimal DiscountValue { get; set; }
}
