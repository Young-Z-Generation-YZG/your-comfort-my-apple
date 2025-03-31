
using YGZ.Identity.Domain.Core.Primitives;

namespace YGZ.Identity.Domain.Users.ValueObjects;

public class Image : ValueObject
{
    public string ImageId { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    private Image(string id, string url)
    {
        ImageId = id;
        ImageUrl = url;
    }

    private Image() { }

    public static Image Create(string imageId,
                               string imageUrl)
    {
        return new Image(imageId, imageUrl);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ImageId;
        yield return ImageUrl;
    }
}
