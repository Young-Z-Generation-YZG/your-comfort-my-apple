
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YGZ.Identity.Application.Abstractions;
using YGZ.Identity.Domain.Authorizations;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Infrastructure.Settings;

namespace YGZ.Identity.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _secretKey;
    private readonly string _validIssuer;
    private readonly string _validAudience;
    private readonly double _expireSeconds;

    private readonly UserManager<User> _userManager;

    public TokenService(IOptions<JwtSettings> settings, UserManager<User> userManager)
    {
        if (settings.Value is null || string.IsNullOrWhiteSpace(settings.Value.SecretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured");
        }

        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.SecretKey));
        _validIssuer = settings.Value.ValidIssuer;
        _validAudience = settings.Value.ValidAudience;
        _expireSeconds = settings.Value.ExpiredSeconds;
        _userManager = userManager;
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

        var claims = await GetClaimsAsync(user);

        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public Task<string> GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _validIssuer,
            audience: _validAudience,
            claims: claims,
            expires: DateTime.Now.AddSeconds(_expireSeconds),
            signingCredentials: signingCredentials
            );
    }

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Scope, ApiScope.Read)
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
