﻿namespace YGZ.Identity.Api.Contracts.Auth;

public class AccessOtpRequest
{
    public string? _email { get; set; }
    public string? _token { get; set; }
    public string? _verifyType { get; set; }
}
