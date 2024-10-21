
namespace YGZ.Identity.Domain.IdentityServer;

public static class ApiScope
{
    public const string Read = $"{ApiResource.YGZECommerce}_read";
    public const string Write = $"{ApiResource.YGZECommerce}_write";
    public const string Delete = $"{ApiResource.YGZECommerce}_delete";
    public const string Update = $"{ApiResource.YGZECommerce}_update";
    public const string Full = $"{ApiResource.YGZECommerce}_full";
}
