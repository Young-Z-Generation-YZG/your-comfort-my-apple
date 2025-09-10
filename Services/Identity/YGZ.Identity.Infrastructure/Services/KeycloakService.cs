
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
using System.Net;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Application.Keycloak.Commands;

namespace YGZ.Identity.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly KeycloakSettings _keycloakSettings;
    private readonly HttpClient _httpClient;

    private readonly string _nextjsClientUUID;
    private readonly string _nextjsClientId;
    private readonly string _nextjsClientSecret;

    private readonly string _adminClientId;
    private readonly string _adminClientSecret;

    private readonly string _tokenEndpoint;
    private readonly string _adminEndpoint;

    public KeycloakService(HttpClient httpClient, IOptions<KeycloakSettings> options, ILogger<KeycloakService> logger)
    {
        _keycloakSettings = options.Value;
        _httpClient = httpClient;

        _nextjsClientUUID = _keycloakSettings.NextjsClient.ClientUUID;
        _nextjsClientId = _keycloakSettings.NextjsClient.ClientId;
        _nextjsClientSecret = _keycloakSettings.NextjsClient.ClientSecret;

        _adminClientId = _keycloakSettings.AdminClient.ClientId;
        _adminClientSecret = _keycloakSettings.AdminClient.ClientSecret;

        _tokenEndpoint = $"{_keycloakSettings.AuthServerUrl}realms/{_keycloakSettings.Realm}/protocol/openid-connect/token";
        _adminEndpoint = $"{_keycloakSettings.AuthServerUrl}admin/realms/{_keycloakSettings.Realm}/users";
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
        catch (Exception ex)
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
        catch (Exception ex)
        {
            throw;
        }

        return Errors.Keycloak.UserNotFound;
    }

    public async Task<Result<string>> CreateKeycloakUserAsync(RegisterCommand request)
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

        string? userId = null;

        var adminToken = await GetAdminTokenResponseAsync();

        try
        {

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
        catch (Exception ex)
        {
            throw;
        }

        try
        {
            var initRole = new[]
            {
                new
                {
                    id = "11118cf4-b9d1-430d-96c1-4e5272d6d112",
                    name = "USER",
                    description = "",
                    composite = false
                }
            };

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_adminEndpoint}/{userId}/role-mappings/clients/{_nextjsClientUUID}"),
                Content = new StringContent(JsonConvert.SerializeObject(initRole), Encoding.UTF8, "application/json"),
                Headers =
                {
                    { "Authorization", $"Bearer {adminToken}" }
                }
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                return Errors.Keycloak.CannotAssignRole;
            } 
        }
        catch (Exception ex)
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
        catch (Exception ex)
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
        catch (Exception ex)
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
        catch (Exception ex)
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
        catch (Exception ex)
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
        catch (Exception ex)
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
        catch (Exception ex)
        {
            return null;
        }
    }
}
