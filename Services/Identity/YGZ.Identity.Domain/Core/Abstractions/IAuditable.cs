
namespace YGZ.Identity.Domain.Core.Abstractions;

public interface IAuditable
{
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; set; }
}
