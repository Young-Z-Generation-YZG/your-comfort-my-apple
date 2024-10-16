
namespace YGZ.Identity.Domain.Core.Configs;

public class AppConfig
{
  public FrontendConfig FrontendConfig { get; set; } = default!;
}

public class FrontendConfig
{
    public string Url { get; set; } = default!;
}
