
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Keycloak.Application.Abstractions;
using YGZ.Keycloak.Infrastructure.Settings;

namespace YGZ.Keycloak.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _tokenEndpoint;

    public KeycloakService(HttpClient httpClient, IOptions<KeycloakSettings> options)
    {
        _httpClient = httpClient;
        _clientId = options.Value.NextjsClient.ClientId;
        _clientSecret = options.Value.NextjsClient.ClientSecret;
        _tokenEndpoint = $"{options.Value.AuthServerUrl}realms/{options.Value.Realm}/protocol/openid-connect/token";
    }


    public async Task<TokenResponse> GetTokenClientCredentialsAsync()
    {
        try
        {
            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret)
            });

            var response = await _httpClient.PostAsync(_tokenEndpoint, requestBody);

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new InvalidOperationException("Failed to retrieve a valid access token.");
            }

            return tokenResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
