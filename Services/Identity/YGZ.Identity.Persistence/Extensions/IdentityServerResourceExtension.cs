
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using YGZ.Identity.Domain.Core.Configs;
using DomainIdentityServerConstants = YGZ.Identity.Domain.IdentityServer.Authorization;

namespace YGZ.Identity.Persistence.Extensions;

public class IdentityServerResourceExtension
{
    /// <summary>
    /// Returns a collection of API scopes representing different permissions for accessing endpoints.
    /// </summary>
    /// <returns>An enumerable collection of API scopes.</returns>
    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new("api1")
        //new()
        //{
        //    Name = DomainIdentityServerConstants.ApiScope.Read,
        //    DisplayName = DomainIdentityServerConstants.ApiScope.Read,
        //    Description = "Authorized to use all GET endpoints",
        //    Required = true,
        //    UserClaims = new List<string> { JwtClaimTypes.Email }
        //},
        //new()
        //{
        //    Name = DomainIdentityServerConstants.ApiScope.Write,
        //    DisplayName = DomainIdentityServerConstants.ApiScope.Write,
        //    Description = "Authorized to use all POST endpoints",
        //    Required = true,
        //    UserClaims = new List<string> { JwtClaimTypes.Email, }
        //},
        //new()
        //{
        //    Name = DomainIdentityServerConstants.ApiScope.Update,
        //    DisplayName = DomainIdentityServerConstants.ApiScope.Update,
        //    Description = "Authorized to use all PUT, PATCH endpoints",
        //    Required = true,
        //    UserClaims = new List<string> { JwtClaimTypes.Email, }
        //},
        //new()
        //{
        //    Name = DomainIdentityServerConstants.ApiScope.Delete,
        //    DisplayName = DomainIdentityServerConstants.ApiScope.Delete,
        //    Description = "Authorized to use all DELETE endpoints",
        //    Required = true,
        //    UserClaims = new List<string> { JwtClaimTypes.Email, }
        //},
        // new()
        // {
        //    Name = DomainIdentityServerConstants.ApiScope.Test,
        //    DisplayName = DomainIdentityServerConstants.ApiScope.Test,
        //    Description = "Testing Scope",
        //    Required = false,
        //    UserClaims = new List<string> { JwtClaimTypes.Email, }
        // },
    };


    /// <summary>
    /// Returns a collection of API resources representing different resources and associated scopes.
    /// </summary>
    /// <returns>An enumerable collection of API resources.</returns>
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new()
            {
                Name = DomainIdentityServerConstants.ApiResource.YGZECommerce,
                DisplayName = DomainIdentityServerConstants.ApiResource.YGZECommerce,
                Description = "E-commerce API",
                Scopes = new List<string>
                {
                    DomainIdentityServerConstants.ApiScope.Read,
                    DomainIdentityServerConstants.ApiScope.Write,
                    DomainIdentityServerConstants.ApiScope.Update,
                    DomainIdentityServerConstants.ApiScope.Delete,
                    DomainIdentityServerConstants.ApiScope.Test
                },
                UserClaims = new List<string> 
                { 
                    JwtClaimTypes.Email,
                    JwtClaimTypes.GivenName,
                    JwtClaimTypes.FamilyName,
                }
            }
        };


    public static IEnumerable<Client> Clients(AppConfig configuration)
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "web_client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec261e".Sha256()) },

                AllowedScopes = { "scope1" }
            },
            new Client
            {
                ClientId = "web",
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5002/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "api1" }
            }
            //new()
            //{
            //    ClientId = DomainIdentityServerConstants.ClientId.WebClient,
            //    ClientName = DomainIdentityServerConstants.ClientName.WebClient,
            //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            //    RequireClientSecret = true,
            //    AlwaysSendClientClaims = true,
            //    AlwaysIncludeUserClaimsInIdToken = true,
            //    AllowedScopes =
            //    {
            //        DomainIdentityServerConstants.ApiScope.Read,
            //        DomainIdentityServerConstants.ApiScope.Write,
            //        DomainIdentityServerConstants.ApiScope.Update,
            //        DomainIdentityServerConstants.ApiScope.Delete,
            //        DomainIdentityServerConstants.ApiScope.Test,
            //        IdentityServerConstants.StandardScopes.Profile,
            //        IdentityServerConstants.StandardScopes.OpenId,
            //        IdentityServerConstants.StandardScopes.OfflineAccess,
            //        IdentityServerConstants.StandardScopes.Email
            //    },
            //},
            //new()
            //{
            //    ClientId = "nextjs_client",
            //    ClientName = "NextJS Client",
            //    AllowedGrantTypes = GrantTypes.Code,

            //    RedirectUris = new List<string>
            //    {
            //        "http://localhost:3000/signin-callback",
            //        "http://localhost:3000/silent-callback.html",
            //        "http://localhost:3000/signin-oidc"
            //    },
            //    RequirePkce = true,
            //    AllowAccessTokensViaBrowser = true,
            //    Enabled = true,
            //    UpdateAccessTokenClaimsOnRefresh = true,

            //    AllowedScopes =
            //    {
            //        IdentityServerConstants.StandardScopes.Profile,
            //        IdentityServerConstants.StandardScopes.OpenId,
            //    },
            //    AllowedCorsOrigins = { "http://localhost:3000" },
            //    RequireClientSecret = false,
            //    RequireConsent = false,
            //   AccessTokenLifetime = 3600,
            //   PostLogoutRedirectUris = new List<string>
            //   {
            //        "http://localhost:4200/signout-callback",
            //        "https://localhost:3001/signout-callback-oidc"
            //   },
            //   ClientSecrets = new List<Secret>
            //   {
            //       new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec26d2".Sha256())
            //   }
            //},
            //new()
            //{
            //    ClientId = "nextjs_client_2",
            //    ClientName = "NextJS Client 2",
            //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            //    RequireClientSecret = true,
            //    AlwaysSendClientClaims = true,
            //    AlwaysIncludeUserClaimsInIdToken = true,
            //    AllowedScopes =
            //    {
            //        IdentityServerConstants.StandardScopes.Profile,
            //        IdentityServerConstants.StandardScopes.OpenId,
            //        IdentityServerConstants.StandardScopes.OfflineAccess,
            //        IdentityServerConstants.StandardScopes.Email,
            //    },
            //    ClientSecrets = new List<Secret>
            //    {
            //        new Secret("5c6eb3b4-61a7-4668-ac57-2b4591ec261e".Sha256())
            //    },
            //    AllowOfflineAccess = true,
            //    RefreshTokenUsage = TokenUsage.OneTimeOnly,
            //    Enabled = true,
            //},
        };
    }

    /// <summary>
    /// Returns a collection of identity resources representing standard OpenID Connect identity scopes.
    /// </summary>
    /// <returns>An enumerable collection of identity resources.</returns>
    public static IEnumerable<IdentityResource> IdentityResources =>
    new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    };
}
