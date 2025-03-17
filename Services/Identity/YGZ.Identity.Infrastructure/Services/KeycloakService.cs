
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Infrastructure.Settings;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly ILogger<KeycloakService> _logger;
    private readonly KeycloakSettings _keycloakSettings;
    private readonly HttpClient _httpClient;

    private readonly string _nextjsClientId;
    private readonly string _nextjsClientSecret;

    private readonly string _adminClientId;
    private readonly string _adminClientSecret;

    private readonly string _tokenEndpoint;
    private readonly string _adminEndpoint;

    public KeycloakService(HttpClient httpClient, IOptions<KeycloakSettings> options, ILogger<KeycloakService> logger)
    {
        _logger = logger;
        _keycloakSettings = options.Value;
        _httpClient = httpClient;

        _nextjsClientId = _keycloakSettings.NextjsClient.ClientId;
        _nextjsClientSecret = _keycloakSettings.NextjsClient.ClientSecret;

        _adminClientId = _keycloakSettings.AdminClient.ClientId;
        _adminClientSecret = _keycloakSettings.AdminClient.ClientSecret;

        _tokenEndpoint = $"{_keycloakSettings.AuthServerUrl}realms/{_keycloakSettings.Realm}/protocol/openid-connect/token";
        _adminEndpoint = $"{_keycloakSettings.AuthServerUrl}admin/realms/{_keycloakSettings.Realm}/users";
    }

    public async Task<Result<bool>> CreateKeycloakUserAsync(RegisterCommand request)
    {
        var keycloakUser = new
        {
            username = request.Email,
            email = request.Email,
            enabled = true,
            firstName = request.FirstName,
            lastName = request.LastName,
            emailVerified = false,
            credentials = new[]
            {
                new
                {
                    type = "password",
                    value = request.Password,
                    temporary = false
                }
            }
        };

        var adminToken = await GetAdminTokenResponseAsync();

        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_adminEndpoint),
            Content = new StringContent(JsonConvert.SerializeObject(keycloakUser), Encoding.UTF8, "application/json"),
            Headers =
            {
                { "Authorization", $"Bearer {adminToken}" }
            }
        };

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(CreateKeycloakUserAsync));
            throw;
        }

        return Errors.Keycloak.CannotBeCreated;
    }

    public async Task<string> GetKeycloackUserTokenAsync(LoginCommand request)
    {
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", OpenIdConnectGrantTypes.Password),
            new KeyValuePair<string, string>("username", request.Email),
            new KeyValuePair<string, string>("password", request.Password),
            new KeyValuePair<string, string>("client_id", _nextjsClientId),
            new KeyValuePair<string, string>("client_secret", _nextjsClientSecret)
        });

        try
        {
            var response = await _httpClient.PostAsync(_tokenEndpoint, requestBody);

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (tokenResponse == null && string.IsNullOrWhiteSpace(tokenResponse!.AccessToken))
            {
                throw new Exception("Failed to retrieve user token.");
            }

            return tokenResponse.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GetKeycloackUserTokenAsync));
            throw;
        }
    }

    public async Task<TokenResponse> GetTokenClientCredentialsTypeAsync()
    {
        try
        {
            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _nextjsClientId),
                new KeyValuePair<string, string>("client_secret", _nextjsClientSecret)
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

    private async Task<string> GetAdminTokenResponseAsync()
    {
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", OpenIdConnectGrantTypes.ClientCredentials),
            new KeyValuePair<string, string>("client_id", _adminClientId),
            new KeyValuePair<string, string>("client_secret", _adminClientSecret)
        });

        try
        {
            var response = await _httpClient.PostAsync(_tokenEndpoint, requestBody);

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (tokenResponse == null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                throw new Exception("Failed to retrieve admin token.");
            }

            return tokenResponse.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GetAdminTokenResponseAsync));
            throw;
        }
    }
}
