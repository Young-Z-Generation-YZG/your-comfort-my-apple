# Keycloak Identity Server Extension Documentation

## Overview

The **Keycloak Identity Server Extension** provides standardized authentication and authorization setup for all microservices in the YGZ system. It integrates Keycloak as the central identity provider using OpenID Connect (OIDC) and JWT Bearer tokens.

## Endpoints

- **Standard OIDC Discovery endpoint**: `/.well-known/openid-configuration`
- **OpenID Connect (OIDC) Discovery endpoint**: `/realms/ygz-realm/.well-known/openid-configuration`

These endpoints provide JSON documents (the "OpenID Provider Metadata") containing configuration details about the OIDC provider, such as supported endpoints, authentication methods, and token formats.

## 🔧 Integration Guide

### **1. Prerequisites**

Before integrating Keycloak, ensure you have:

- ✅ **Keycloak Server** running and accessible
- ✅ **Realm** created (e.g., `ygz-realm`)
- ✅ **Client** configured in Keycloak
- ✅ **Users** and **Roles** set up
- ✅ **Client Secret** generated

### **2. Keycloak Server Setup**

#### **Create Realm**

1. Log into Keycloak Admin Console
2. Click "Create Realm"
3. Enter realm name: `ygz-realm`
4. Click "Create"

#### **Create Client**

1. In your realm, go to "Clients" → "Create"
2. Configure client settings:
   ```
   Client ID: ygz-microservices
   Client Protocol: openid-connect
   Access Type: confidential
   Valid Redirect URIs: http://localhost:5000/*
   Web Origins: http://localhost:3000
   ```
3. Save and go to "Credentials" tab
4. Copy the generated "Client Secret"

#### **Create Roles**

1. Go to "Roles" → "Create Role"
2. Create roles:
   - `[ROLE]:USER` - Basic user access
   - `[ROLE]:ADMIN` - Administrative access
   - `[ROLE]:CLIENT` - Client application access

#### **Assign Roles to Users**

1. Go to "Users" → Select user → "Role Mappings"
2. Assign appropriate roles to users

### **3. Service Configuration**

#### **Add NuGet Package Reference**

```xml
<PackageReference Include="YGZ.BuildingBlocks.Shared" Version="1.0.0" />
```

#### **Update appsettings.json**

```json
{
  "Keycloak": {
    "Authority": "http://localhost:17070/realms/ygz-realm",
    "Audience": "ygz-microservices",
    "RequireHttpsMetadata": false,
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ValidateIssuerSigningKey": true
  },
  "ConnectionStrings": {
    "IdentityDb": "Host=localhost;Database=ygz_identity;Username=postgres;Password=password",
    "KeycloakDb": "Host=localhost;Database=keycloak;Username=postgres;Password=password"
  }
}
```

#### **Update appsettings.Development.json**

```json
{
  "Keycloak": {
    "Authority": "http://localhost:17070/realms/ygz-realm",
    "Audience": "ygz-microservices",
    "RequireHttpsMetadata": false,
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ValidateIssuerSigningKey": true
  },
  "ConnectionStrings": {
    "IdentityDb": "Host=localhost;Database=ygz_identity;Username=postgres;Password=password",
    "KeycloakDb": "Host=localhost;Database=keycloak;Username=postgres;Password=password"
  }
}
```

### **4. Service Registration**

#### **In DependencyInjection.cs**

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Keycloak Authentication
        services.AddKeycloakIdentityServerExtension(configuration);

        // Add OpenTelemetry with Keycloak instrumentation
        services.AddKeycloakOpenTelemetryExtension();

        // Other infrastructure services...
        return services;
    }
}
```

#### **In Program.cs**

```csharp
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add Layers
services
    .AddPresentationLayer(builder)
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer(builder.Configuration);

// ... other service registrations

var app = builder.Build();

// ... other middleware

// Authentication & Authorization (ORDER MATTERS!)
app.UseAuthentication();  // Must come first
app.UseAuthorization();   // Must come after Authentication

app.MapControllers();
app.Run();
```

### **5. Controller Authorization**

#### **Basic Authorization**

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender) { }

    /// <summary>
    /// Get user profile - requires authentication
    /// </summary>
    [HttpGet("profile")]
    [Authorize] // Requires valid JWT token
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var query = new GetUserProfileQuery(userId);
        var result = await Sender.Send(query);

        return result.IsSuccess ? Ok(result.Response) : HandleFailure(result);
    }
}
```

#### **Role-Based Authorization**

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AdminController : ApiController
{
    public AdminController(ISender sender) : base(sender) { }

    /// <summary>
    /// Get all users - requires admin role
    /// </summary>
    [HttpGet("users")]
    [Authorize(Roles = "[ROLE]:ADMIN")] // Requires admin role
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await Sender.Send(query);

        return result.IsSuccess ? Ok(result.Response) : HandleFailure(result);
    }
}
```

#### **Policy-Based Authorization**

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class OrdersController : ApiController
{
    public OrdersController(ISender sender) : base(sender) { }

    /// <summary>
    /// Create order - requires client role policy
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationConstants.Policies.RoleStaff)]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var command = request.Adapt<CreateOrderCommand>();
        var result = await Sender.Send(command);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetOrder), new { id = result.Response }, result.Response)
            : HandleFailure(result);
    }
}
```

### **6. JWT Token Handling**

#### **Token Validation**

The extension automatically validates JWT tokens:

- ✅ **Signature verification** using Keycloak's public keys
- ✅ **Issuer validation** against configured authority
- ✅ **Audience validation** against configured audience
- ✅ **Expiration time** validation
- ✅ **Not before time** validation

#### **Token Claims**

Access user information from JWT claims:

```csharp
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string GetCurrentUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Email)?.Value;
    }

    public IEnumerable<string> GetCurrentUserRoles()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value) ?? Enumerable.Empty<string>();
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User
            .IsInRole(role) ?? false;
    }
}
```

### **7. Health Checks**

#### **Add Keycloak Health Check**

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddMonitoringAndLogging(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddHealthChecks()
            .AddNpgSql(
                connectionString: builder.Configuration.GetConnectionString("IdentityDb")!,
                name: "IdentityDb",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "postgres" })
            .AddNpgSql(
                connectionString: builder.Configuration.GetConnectionString("KeycloakDb")!,
                name: "KeycloakDb",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "postgres" });

        return services;
    }
}
```

### **8. Testing Integration**

#### **Test Authentication Endpoint**

```bash
# Get token from Keycloak
curl -X POST "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password&client_id=ygz-microservices&client_secret=YOUR_CLIENT_SECRET&username=testuser&password=testpass"

# Use token to access protected endpoint
curl -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  "http://localhost:5000/api/v1/users/profile"
```

#### **Test Health Check**

```bash
curl "http://localhost:5000/health"
```

Expected response:

```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456",
  "entries": {
    "IdentityDb": {
      "status": "Healthy",
      "duration": "00:00:00.0045678"
    },
    "KeycloakDb": {
      "status": "Healthy",
      "duration": "00:00:00.0034567"
    }
  }
}
```

### **9. Troubleshooting**

#### **Common Issues**

**1. JWT Token Validation Failed**

```
System.ArgumentOutOfRangeException: IDX10634: Unable to create the SignatureProvider
```

**Solution**: Check Keycloak authority URL and ensure it's accessible.

**2. Audience Validation Failed**

```
IDX10214: Audience validation failed
```

**Solution**: Verify the audience in appsettings.json matches the client ID in Keycloak.

**3. Issuer Validation Failed**

```
IDX10205: Issuer validation failed
```

**Solution**: Ensure the authority URL in appsettings.json matches your Keycloak realm URL.

**4. Token Expired**

```
IDX10223: Lifetime validation failed
```

**Solution**: Check system clock synchronization and token expiration settings in Keycloak.

#### **Debug Configuration**

Enable detailed logging in `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore.Authentication": "Debug",
      "Microsoft.AspNetCore.Authorization": "Debug"
    }
  }
}
```

### **10. Security Best Practices**

1. ✅ **Use HTTPS in Production**: Set `RequireHttpsMetadata` to `true`
2. ✅ **Validate All Claims**: Enable all validation options
3. ✅ **Use Strong Client Secrets**: Generate cryptographically strong secrets
4. ✅ **Implement Token Refresh**: Handle token expiration gracefully
5. ✅ **Log Authentication Events**: Monitor for suspicious activities
6. ✅ **Regular Key Rotation**: Update Keycloak keys periodically
7. ✅ **Role-Based Access Control**: Implement least privilege principle
8. ✅ **Audit Trail**: Log all authorization decisions

### **11. Performance Considerations**

1. **Token Caching**: Implement token caching to reduce Keycloak calls
2. **Connection Pooling**: Use connection pooling for database connections
3. **Health Check Frequency**: Configure appropriate health check intervals
4. **Token Validation**: Consider implementing token validation caching

---

## 📋 **Integration Checklist**

- [ ] Keycloak server running and accessible
- [ ] Realm created (`ygz-realm`)
- [ ] Client configured with proper settings
- [ ] Client secret generated and secured
- [ ] Users and roles created
- [ ] NuGet package reference added
- [ ] Configuration updated in appsettings.json
- [ ] Service registration in DependencyInjection.cs
- [ ] Middleware order correct in Program.cs
- [ ] Controllers protected with [Authorize] attributes
- [ ] Health checks configured
- [ ] Testing completed
- [ ] Security review performed
- [ ] Documentation updated

---

## 🔗 **Additional Resources**

- [Keycloak Documentation](https://www.keycloak.org/documentation)
- [OpenID Connect Specification](https://openid.net/connect/)
- [JWT Token Structure](https://jwt.io/introduction)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [BuildingBlocks Documentation](./building-blocks.md)

The Keycloak Identity Server Extension provides a robust, secure, and standardized authentication solution for the YGZ microservices ecosystem, ensuring consistent security practices across all services.

```json
{
  "issuer": "http://localhost:17070/realms/ygz-realm",
  "authorization_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/auth",
  "token_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/token",
  "introspection_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/token/introspect",
  "userinfo_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/userinfo",
  "end_session_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/logout",
  "frontchannel_logout_session_supported": true,
  "frontchannel_logout_supported": true,
  "jwks_uri": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/certs",
  "check_session_iframe": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/login-status-iframe.html",
  "grant_types_supported": [
    "authorization_code",
    "implicit",
    "refresh_token",
    "password",
    "client_credentials",
    "urn:openid:params:grant-type:ciba",
    "urn:ietf:params:oauth:grant-type:device_code"
  ],
  "acr_values_supported": ["0", "1"],
  "response_types_supported": [
    "code",
    "none",
    "id_token",
    "token",
    "id_token token",
    "code id_token",
    "code token",
    "code id_token token"
  ],
  "subject_types_supported": ["public", "pairwise"],
  "prompt_values_supported": ["none", "login", "consent"],
  "id_token_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "HS256",
    "HS512",
    "ES256",
    "RS256",
    "HS384",
    "ES512",
    "PS256",
    "PS512",
    "RS512"
  ],
  "id_token_encryption_alg_values_supported": [
    "ECDH-ES+A256KW",
    "ECDH-ES+A192KW",
    "ECDH-ES+A128KW",
    "RSA-OAEP",
    "RSA-OAEP-256",
    "RSA1_5",
    "ECDH-ES"
  ],
  "id_token_encryption_enc_values_supported": [
    "A256GCM",
    "A192GCM",
    "A128GCM",
    "A128CBC-HS256",
    "A192CBC-HS384",
    "A256CBC-HS512"
  ],
  "userinfo_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "HS256",
    "HS512",
    "ES256",
    "RS256",
    "HS384",
    "ES512",
    "PS256",
    "PS512",
    "RS512",
    "none"
  ],
  "userinfo_encryption_alg_values_supported": [
    "ECDH-ES+A256KW",
    "ECDH-ES+A192KW",
    "ECDH-ES+A128KW",
    "RSA-OAEP",
    "RSA-OAEP-256",
    "RSA1_5",
    "ECDH-ES"
  ],
  "userinfo_encryption_enc_values_supported": [
    "A256GCM",
    "A192GCM",
    "A128GCM",
    "A128CBC-HS256",
    "A192CBC-HS384",
    "A256CBC-HS512"
  ],
  "request_object_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "HS256",
    "HS512",
    "ES256",
    "RS256",
    "HS384",
    "ES512",
    "PS256",
    "PS512",
    "RS512",
    "none"
  ],
  "request_object_encryption_alg_values_supported": [
    "ECDH-ES+A256KW",
    "ECDH-ES+A192KW",
    "ECDH-ES+A128KW",
    "RSA-OAEP",
    "RSA-OAEP-256",
    "RSA1_5",
    "ECDH-ES"
  ],
  "request_object_encryption_enc_values_supported": [
    "A256GCM",
    "A192GCM",
    "A128GCM",
    "A128CBC-HS256",
    "A192CBC-HS384",
    "A256CBC-HS512"
  ],
  "response_modes_supported": [
    "query",
    "fragment",
    "form_post",
    "query.jwt",
    "fragment.jwt",
    "form_post.jwt",
    "jwt"
  ],
  "registration_endpoint": "http://localhost:17070/realms/ygz-realm/clients-registrations/openid-connect",
  "token_endpoint_auth_methods_supported": [
    "private_key_jwt",
    "client_secret_basic",
    "client_secret_post",
    "tls_client_auth",
    "client_secret_jwt"
  ],
  "token_endpoint_auth_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "HS256",
    "HS512",
    "ES256",
    "RS256",
    "HS384",
    "ES512",
    "PS256",
    "PS512",
    "RS512"
  ],
  "introspection_endpoint_auth_methods_supported": [
    "private_key_jwt",
    "client_secret_basic",
    "client_secret_post",
    "tls_client_auth",
    "client_secret_jwt"
  ],
  "introspection_endpoint_auth_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "HS256",
    "HS512",
    "ES256",
    "RS256",
    "HS384",
    "ES512",
    "PS256",
    "PS512",
    "RS512"
  ],
  "authorization_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "HS256",
    "HS512",
    "ES256",
    "RS256",
    "HS384",
    "ES512",
    "PS256",
    "PS512",
    "RS512"
  ],
  "authorization_encryption_alg_values_supported": [
    "ECDH-ES+A256KW",
    "ECDH-ES+A192KW",
    "ECDH-ES+A128KW",
    "RSA-OAEP",
    "RSA-OAEP-256",
    "RSA1_5",
    "ECDH-ES"
  ],
  "authorization_encryption_enc_values_supported": [
    "A256GCM",
    "A192GCM",
    "A128GCM",
    "A128CBC-HS256",
    "A192CBC-HS384",
    "A256CBC-HS512"
  ],
  "claims_supported": [
    "aud",
    "sub",
    "iss",
    "auth_time",
    "name",
    "given_name",
    "family_name",
    "preferred_username",
    "email",
    "acr"
  ],
  "claim_types_supported": ["normal"],
  "claims_parameter_supported": true,
  "scopes_supported": [
    "openid",
    "basic",
    "address",
    "profile",
    "web-origins",
    "organization",
    "service_account",
    "microprofile-jwt",
    "email",
    "roles",
    "acr",
    "[Realm-Client-Scope]_Audience_Mapping",
    "phone",
    "offline_access"
  ],
  "request_parameter_supported": true,
  "request_uri_parameter_supported": true,
  "require_request_uri_registration": true,
  "code_challenge_methods_supported": ["plain", "S256"],
  "tls_client_certificate_bound_access_tokens": true,
  "revocation_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/revoke",
  "revocation_endpoint_auth_methods_supported": [
    "private_key_jwt",
    "client_secret_basic",
    "client_secret_post",
    "tls_client_auth",
    "client_secret_jwt"
  ],
  "revocation_endpoint_auth_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "HS256",
    "HS512",
    "ES256",
    "RS256",
    "HS384",
    "ES512",
    "PS256",
    "PS512",
    "RS512"
  ],
  "backchannel_logout_supported": true,
  "backchannel_logout_session_supported": true,
  "device_authorization_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/auth/device",
  "backchannel_token_delivery_modes_supported": ["poll", "ping"],
  "backchannel_authentication_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/ext/ciba/auth",
  "backchannel_authentication_request_signing_alg_values_supported": [
    "PS384",
    "RS384",
    "EdDSA",
    "ES384",
    "ES256",
    "RS256",
    "ES512",
    "PS256",
    "PS512",
    "RS512"
  ],
  "require_pushed_authorization_requests": false,
  "pushed_authorization_request_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/ext/par/request",
  "mtls_endpoint_aliases": {
    "token_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/token",
    "revocation_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/revoke",
    "introspection_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/token/introspect",
    "device_authorization_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/auth/device",
    "registration_endpoint": "http://localhost:17070/realms/ygz-realm/clients-registrations/openid-connect",
    "userinfo_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/userinfo",
    "pushed_authorization_request_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/ext/par/request",
    "backchannel_authentication_endpoint": "http://localhost:17070/realms/ygz-realm/protocol/openid-connect/ext/ciba/auth"
  },
  "authorization_response_iss_parameter_supported": true
}
```
