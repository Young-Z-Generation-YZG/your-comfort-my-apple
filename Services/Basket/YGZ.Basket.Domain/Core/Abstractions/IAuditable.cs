
namespace YGZ.Basket.Domain.Core.Abstractions;

public interface IAuditable
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}


