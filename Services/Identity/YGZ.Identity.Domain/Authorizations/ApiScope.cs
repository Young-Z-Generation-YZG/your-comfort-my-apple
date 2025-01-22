
namespace YGZ.Identity.Domain.Authorizations;

public static class ApiScope
{
    public const string Read = $"{ApiResource.YGZEcommerce}_read";
    public const string Write = $"{ApiResource.YGZEcommerce}_write";
    public const string Update = $"{ApiResource.YGZEcommerce}_update";
    public const string Delete = $"{ApiResource.YGZEcommerce}_delete";
    public const string Test = $"{ApiResource.YGZEcommerce}_test";
}
