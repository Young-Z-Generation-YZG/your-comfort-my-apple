using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts;

public sealed record VerifyEmailRequest([property: JsonPropertyName("email")] string Email,
                                        [property: JsonPropertyName("token")] string Token,
                                        [property: JsonPropertyName("otp")] string Otp) { }