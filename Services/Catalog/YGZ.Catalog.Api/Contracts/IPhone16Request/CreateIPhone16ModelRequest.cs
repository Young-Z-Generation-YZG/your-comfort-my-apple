using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YGZ.Catalog.Api.Contracts.Common;

namespace YGZ.Catalog.Api.Contracts.IPhone16;

#pragma warning disable CS8618
public sealed record CreateIPhone16ModelRequest()
{
    [Required]
    [JsonPropertyName("models")]
    public List<ModelRequest> Models { get; set; }

    [Required]
    [JsonPropertyName("colors")]
    public List<ColorRequest> Colors { get; set; }

    [Required]
    [JsonPropertyName("storages")]
    public List<int> Storages { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [Required]
    [JsonPropertyName("description_images")]
    public List<ImageRequest> DescriptionImages { get; set; }

    [Required]
    [JsonPropertyName("category_id")]
    public string CategoryId { get; set; }
}
