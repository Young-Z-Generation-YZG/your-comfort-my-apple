﻿
namespace YGZ.Identity.Infrastructure.Settings;

public class WebClientSettings
{
    public const string SettingKey = "WebClientSettings";

    public string BaseUrl { get; set; } = default!;
}
