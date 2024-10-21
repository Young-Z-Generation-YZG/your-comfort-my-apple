
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YGZ.Identity.Application.Core.Abstractions.TokenService;
using YGZ.Identity.Domain.Identity.Entities;
using YGZ.Identity.Domain.IdentityServer;

namespace YGZ.Identity.Persistence.Services.Token;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _symmetricSecurityKey;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtClaimTypes.Scope, ApiScope.Read),
            //new Claim(JwtClaimTypes.Scope, ApiScope.Write)
        };

        var credential = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDiscriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credential,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDiscriptor);

        return tokenHandler.WriteToken(token);
    }
}
