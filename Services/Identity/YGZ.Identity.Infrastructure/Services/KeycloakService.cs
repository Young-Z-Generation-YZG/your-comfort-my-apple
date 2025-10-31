
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.BuildingBlocks.Shared.Contracts.Keycloak;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.BuilderClasses;
using YGZ.Identity.Application.Keycloak.Commands.AuthorizationCode;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Core.Keycloak.Entities;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Infrastructure.Settings;

namespace YGZ.Identity.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly ILogger<KeycloakService> _logger;
    private readonly KeycloakSettings _keycloakSettings;
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly string _nextjsClientUUID;
    private readonly string _nextjsClientId;
    private readonly string _nextjsClientSecret;

    private readonly string _adminClientId;
    private readonly string _adminClientSecret;

    private readonly string _tokenEndpoint;
    private readonly string _adminEndpoint;
    private readonly string _roleManagementEndpoint;

    public KeycloakService(HttpClient httpClient, IOptions<KeycloakSettings> options, ILogger<KeycloakService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _keycloakSettings = options.Value;
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;

        _nextjsClientUUID = _keycloakSettings.NextjsClient.ClientUUID;
        _nextjsClientId = _keycloakSettings.NextjsClient.ClientId;
        _nextjsClientSecret = _keycloakSettings.NextjsClient.ClientSecret;

        _adminClientId = _keycloakSettings.AdminClient.ClientId;
        _adminClientSecret = _keycloakSettings.AdminClient.ClientSecret;

        _tokenEndpoint = $"{_keycloakSettings.AuthServerUrl}realms/{_keycloakSettings.Realm}/protocol/openid-connect/token";
        _adminEndpoint = $"{_keycloakSettings.AuthServerUrl}admin/realms/{_keycloakSettings.Realm}/users";
        _roleManagementEndpoint = $"{_keycloakSettings.AuthServerUrl}admin/realms/{_keycloakSettings.Realm}/clients/{_nextjsClientUUID}/roles";
    }

    public async Task<Result<KeycloakUser>> GetUserByIdAsync(Guid userId)
    {
        var adminToken = await GetAdminTokenResponseAsync();
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_adminEndpoint}/{userId.ToString()}"),
            Headers =
        {
            { "Authorization", $"Bearer {adminToken}" },
            { "Accept", "application/json" }
        }
        };

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<KeycloakUser>>(content);
                var user = users.Find(u => u.Id == userId.ToString());

                return user!;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return null!;
    }

    public async Task<Result<KeycloakUser?>> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
    {
        var adminToken = await GetAdminTokenResponseAsync();
        var url = new Uri(_adminEndpoint + "?q=" + usernameOrEmail);
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = url,
            Headers =
            {
                { "Authorization", $"Bearer {adminToken}" },
                { "Accept", "application/json" }
            }
        };

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<KeycloakUser>>(content);
                var user = users.Find(u => u.Email == usernameOrEmail);

                if (user != null)
                {
                    return user;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }

        return Errors.Keycloak.UserNotFound;
    }

    public async Task<Result<string>> CreateKeycloakUserAsync(UserRepresentation userRepresentation)
    {
        var keycloakUser = userRepresentation;

        var jsonContent = new StringContent(JsonConvert.SerializeObject(keycloakUser), Encoding.UTF8, "application/json");

        string? userId = null;

        var adminToken = await GetAdminTokenResponseAsync();

        try
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_adminEndpoint),
                Content = jsonContent,
                Headers =
                {
                    { "Authorization", $"Bearer {adminToken}" }
                }
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                if (response.Headers.Contains("Location"))
                {
                    var locationHeader = response.Headers.GetValues("Location").First();
                    var locationUrl = new Uri(locationHeader);
                    var path = locationUrl.AbsolutePath;
                    var parts = path.TrimStart('/').Split('/');
                    int usersIndex = Array.IndexOf(parts, "users");
                    if (usersIndex != -1 && usersIndex + 1 < parts.Length)
                    {
                        userId = parts[usersIndex + 1];

                    }
                    else
                    {

                        return Errors.Keycloak.CannotBeCreated;
                    }
                }
                else
                {

                    return Errors.Keycloak.CannotBeCreated;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }

        try
        {
            var keycloakRole = await GetKeycloakRoleByNameAsync(AuthorizationConstants.Roles.USER);

            List<KeycloakRole> roles = new List<KeycloakRole> { keycloakRole.Response! };

            var roleJson = JsonConvert.SerializeObject(roles);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_adminEndpoint}/{userId}/role-mappings/clients/{_nextjsClientUUID}"),
                Content = new StringContent(roleJson, Encoding.UTF8, "application/json"),
                Headers =
                {
                    { "Authorization", $"Bearer {adminToken}" }
                }
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                await DeleteKeycloakUserAsync(userId!);

                _logger.LogError("Failed to assign role to user {UserId}", userId);
                return Errors.Keycloak.CannotAssignRole;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return userId!;
    }

    public async Task<TokenResponse> GetKeycloakTokenPairAsync(LoginCommand request)
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

            response.EnsureSuccessStatusCode();

            var responseJsonString = await response.Content.ReadAsStringAsync();

            var deserializedResp = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(responseJsonString);

            if (deserializedResp == null)
            {
                throw new Exception("Failed to retrieve user token.");
            }

            return deserializedResp;
        }
        catch (Exception)
        {
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

            var responseJsonString = await response.Content.ReadAsStringAsync();

            var deserializedResp = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(responseJsonString);

            if (deserializedResp == null)
            {
                throw new Exception("Failed to retrieve user token.");
            }

            return deserializedResp.AccessToken;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Result<bool>> VerifyEmailAsync(string email)
    {
        try
        {
            // Step 1: Find the user by email
            var userResult = await GetUserByUsernameOrEmailAsync(email);

            if (userResult.IsFailure || userResult.Response == null)
            {

                return Errors.Keycloak.UserNotFound;
            }

            var userId = userResult.Response.Id;

            // Step 2: Get admin token
            var adminToken = await GetAdminTokenResponseAsync();

            // Step 3: Prepare the update payload (only updating emailVerified)
            var updatePayload = new
            {
                emailVerified = true
            };

            // Step 4: Create the PUT request to update the user
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_adminEndpoint}/{userId}"),
                Content = new StringContent(JsonConvert.SerializeObject(updatePayload), Encoding.UTF8, "application/json"),
                Headers =
            {
                { "Authorization", $"Bearer {adminToken}" },
                { "Accept", "application/json" }
            }
            };

            // Step 5: Send the request
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();


                return Errors.Keycloak.EmailVerificationFailed;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Result<TokenResponse>> RefreshAccessTokenAsync(string refreshToken)
    {
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", OpenIdConnectGrantTypes.RefreshToken),
            new KeyValuePair<string, string>("client_id", _nextjsClientId),
            new KeyValuePair<string, string>("client_secret", _nextjsClientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken)
        });

        try
        {
            var response = await _httpClient.PostAsync(_tokenEndpoint, requestBody);

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (tokenResponse == null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                throw new Exception("Failed to retrieve user token.");
            }
            return tokenResponse;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> SendEmailResetPasswordAsync(string email)
    {
        try
        {
            // Step 1: Find the user by email
            var userResult = await GetUserByUsernameOrEmailAsync(email);
            if (userResult.IsFailure || userResult.Response == null)
            {
                return Errors.Keycloak.UserNotFound;
            }

            var userId = userResult.Response.Id;

            // Step 2: Get admin token
            var adminToken = await GetAdminTokenResponseAsync();

            // Step 3: Prepare the payload to trigger password reset email
            var resetPayload = new
            {
                actions = new[] { "UPDATE_PASSWORD" }
            };

            // Step 4: Create the PUT request to execute the password reset action
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_adminEndpoint}/{userId}/execute-actions-email"),
                Content = new StringContent(JsonConvert.SerializeObject(resetPayload), Encoding.UTF8, "application/json"),
                Headers =
            {
                { "Authorization", $"Bearer {adminToken}" },
                { "Accept", "application/json" }
            }
            };

            // Step 5: Send the request
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                return Errors.Keycloak.SendEmailResetPasswordFailed;
            }
        }
        catch (Exception)
        {
            return Errors.Keycloak.SendEmailResetPasswordFailed;
        }
    }

    public async Task<Result<bool>> ChangePasswordAsync(string email, string currPassword, string newPassword)
    {
        try
        {
            var userResult = await GetUserByUsernameOrEmailAsync(email);
            if (userResult.IsFailure || userResult.Response == null)
            {
                return Errors.Keycloak.UserNotFound;
            }

            var userId = userResult.Response.Id;

            // Step 2: Verify current password (optional, for additional security)
            var loginRequest = new LoginCommand { Email = email, Password = currPassword };
            try
            {
                var tokenResponse = await GetKeycloakTokenPairAsync(loginRequest);
                if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    return Errors.Keycloak.InvalidCredentials;
                }
            }
            catch
            {
                return Errors.Keycloak.InvalidCredentials;
            }

            // Step 3: Get admin token
            var adminToken = await GetAdminTokenResponseAsync();

            // Step 4: Prepare the new password payload
            var resetPayload = new
            {
                credentials = new[]
                {
                    new
                    {
                        type = "password",
                        value = newPassword,
                        temporary = false
                    }
                }
            };

            // Step 5: Create the PUT request to update the user's password
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_adminEndpoint}/{userId}"),
                Content = new StringContent(JsonConvert.SerializeObject(resetPayload), Encoding.UTF8, "application/json"),
                Headers =
            {
                { "Authorization", $"Bearer {adminToken}" },
                { "Accept", "application/json" }
            }
            };

            // Step 6: Send the request
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                return Errors.Keycloak.ChangePasswordFailed;
            }
        }
        catch (Exception)
        {
            return Errors.Keycloak.ChangePasswordFailed;
        }
    }

    public async Task<Result<bool>> ResetPasswordAsync(string email, string newPassword)
    {
        try
        {
            var userResult = await GetUserByUsernameOrEmailAsync(email);
            if (userResult.IsFailure || userResult.Response == null)
            {
                return Errors.Keycloak.UserNotFound;
            }

            var userId = userResult.Response.Id;

            var adminToken = await GetAdminTokenResponseAsync();

            var resetPayload = new
            {
                credentials = new[]
                {
                new
                {
                    type = "password",
                    value = newPassword,
                    temporary = false
                }
            }
            };

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_adminEndpoint}/{userId}"),
                Content = new StringContent(JsonConvert.SerializeObject(resetPayload), Encoding.UTF8, "application/json"),
                Headers =
            {
                { "Authorization", $"Bearer {adminToken}" },
                { "Accept", "application/json" }
            }
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                return Errors.Keycloak.ChangePasswordFailed;
            }
        }
        catch (Exception)
        {
            return Errors.Keycloak.ResetPasswordFailed;
        }
    }

    public async Task<TokenResponse> AuthorizationCode(AuthorizationCodeCommand request)
    {
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", OpenIdConnectGrantTypes.AuthorizationCode),
            new KeyValuePair<string, string>("client_id", _nextjsClientId),
            new KeyValuePair<string, string>("client_secret", _nextjsClientSecret),
            new KeyValuePair<string, string>("code", request.Code),
            new KeyValuePair<string, string>("redirect_uri", "http://localhost:3000/auth/callback")
        });

        try
        {
            var response = await _httpClient.PostAsync(_tokenEndpoint, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                if (tokenResponse == null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                {
                    throw new Exception("Failed to retrieve user token.");
                }
                return tokenResponse;
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Result<bool>> DeleteKeycloakUserAsync(string keycloakUserId)
    {
        try
        {
            var adminToken = await GetAdminTokenResponseAsync();

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_adminEndpoint}/{keycloakUserId}"),
                Headers =
                {
                    { "Authorization", $"Bearer {adminToken}" }
                }
            };

            var response = await _httpClient.SendAsync(requestMessage);

            // According to Keycloak API docs, successful deletion returns 204 No Content
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                _logger.LogInformation("Successfully deleted Keycloak user with ID: {UserId}", keycloakUserId);
                return true;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Keycloak user not found with ID: {UserId}", keycloakUserId);
                return Errors.Keycloak.UserNotFound;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to delete Keycloak user. Status: {StatusCode}, Error: {Error}",
                response.StatusCode, errorContent);

            return Errors.Keycloak.CannotBeDeleted;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request exception while deleting Keycloak user with ID: {UserId}", keycloakUserId);
            return Errors.Keycloak.CannotBeDeleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting Keycloak user with ID: {UserId}", keycloakUserId);
            throw;
        }
    }

    public async Task<Result<KeycloakRole>> GetKeycloakRoleByNameAsync(string roleName)
    {
        try
        {
            var adminToken = await GetAdminTokenResponseAsync();

            // Keycloak API endpoint: GET /admin/realms/{realm}/clients/{id}/roles/{role-name}
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_roleManagementEndpoint}/{roleName}"),
                Headers =
                {
                    { "Authorization", $"Bearer {adminToken}" },
                    { "Accept", "application/json" }
                }
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var role = JsonConvert.DeserializeObject<KeycloakRole>(content);

                if (role == null)
                {
                    _logger.LogWarning("Failed to deserialize Keycloak role: {RoleName}", roleName);
                    return Errors.Keycloak.RoleNotFound;
                }

                _logger.LogInformation("Successfully retrieved Keycloak role: {RoleName} with ID: {RoleId}", roleName, role.Id);
                return role;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Keycloak role not found: {RoleName}", roleName);
                return Errors.Keycloak.RoleNotFound;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to get Keycloak role. Status: {StatusCode}, Error: {Error}",
                response.StatusCode, errorContent);

            return Errors.Keycloak.RoleRetrievalFailed;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request exception while retrieving Keycloak role: {RoleName}", roleName);
            return Errors.Keycloak.RoleRetrievalFailed;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization exception while retrieving Keycloak role: {RoleName}", roleName);
            return Errors.Keycloak.RoleRetrievalFailed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving Keycloak role: {RoleName}", roleName);
            throw;
        }
    }

    public async Task<Result<TokenExchangeResponse>> ImpersonateUserAsync(string userId)
    {
        try
        {
            var subjectToken = await GetAdminTokenResponseAsync();

            // Prepare form-urlencoded request body for token exchange
            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"),
                new KeyValuePair<string, string>("subject_token", subjectToken),
                new KeyValuePair<string, string>("client_id", _adminClientId),
                new KeyValuePair<string, string>("client_secret", _adminClientSecret),
                new KeyValuePair<string, string>("requested_subject", userId)
            });

            _logger.LogInformation("Attempting to impersonate user {UserId}", userId);

            var response = await _httpClient.PostAsync(_tokenEndpoint, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseJsonString = await response.Content.ReadAsStringAsync();
                var tokenExchangeResponse = System.Text.Json.JsonSerializer.Deserialize<TokenExchangeResponse>(responseJsonString);

                if (tokenExchangeResponse == null || string.IsNullOrEmpty(tokenExchangeResponse.AccessToken))
                {
                    _logger.LogError("Failed to deserialize token exchange response for user {UserId}", userId);
                    return Errors.Keycloak.UserNotFound;
                }

                _logger.LogInformation("Successfully impersonated user {UserId}", userId);
                return tokenExchangeResponse;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to impersonate user {UserId}. Status: {StatusCode}, Error: {Error}",
                userId, response.StatusCode, errorContent);

            return Errors.Keycloak.UserNotFound;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request exception while impersonating user {UserId}", userId);
            return Errors.Keycloak.UserNotFound;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while impersonating user {UserId}", userId);
            throw;
        }
    }
}
