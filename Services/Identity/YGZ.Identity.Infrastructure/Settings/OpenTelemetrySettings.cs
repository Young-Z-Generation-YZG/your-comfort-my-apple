
namespace YGZ.Identity.Infrastructure.Settings;

public class OpenTelemetrySettings
{
    public const string SettingKey = "OpenTelemetrySettings";

    public string OtelExporterOtlpEndpointSeq { get; set; } = default!;
    public string OtelExporterOtlpEndpointJaeger { get; set; } = default!;
}
