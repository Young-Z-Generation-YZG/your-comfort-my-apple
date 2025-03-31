
namespace YGZ.Identity.Application.Abstractions.Data;

public interface ICachedRepository
{
    public Task SetAsync(string key, string value, TimeSpan? ttl);
    public Task<string?> GetAsync(string key);
    public Task RemoveAsync(string key);
}
