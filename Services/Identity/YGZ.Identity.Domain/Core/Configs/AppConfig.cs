
namespace YGZ.Identity.Domain.Core.Configs;

public class AppConfig
{
  public FrontendConfig FrontendConfig { get; set; } = default!;
  public IdentityServerConfig IdentityServerConfig { get; set; } = default!;
}

public class IdentityServerConfig
{
    public IdentityServerClients Clients { get; set; } = default!;
    public required string Authority { get; set; } = default!;
    public required string IssuerUrl { get; set; } = default!;
}

public class IdentityServerClients
{
    public InternalClient WebClient { get; set; } = default!;
    public ExternalClient GoogleClient { get; set; } = default!;
}

public class InternalClient
{
    public required string Secret { get; set; } = default!;
}

public class ExternalClient
{
    public required string InternalSecret { get; set; } = default!;
    public required string ExternalClientId { get; set; } = default!;
    public required string ExternalClientSecret { get; set; } = default!;
    public required string RedirectUri { get; set; } = default!;
    public required string PostLogoutRedirectUri { get; set; } = default!;
    public required string AllowedCorsOrigin { get; set; } = default!;
}





public class FrontendConfig
{
    public string Url { get; set; } = default!;
}