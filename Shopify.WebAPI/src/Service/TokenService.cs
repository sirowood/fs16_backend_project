using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;

namespace Shopify.WebAPI.src.Service;

public class TokenService : ITokenService
{
  private readonly IConfiguration _config;

  public TokenService(IConfiguration config)
  {
    _config = config;
  }

  public string GenerateToken(User user)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimTypes.Role, user.Role.ToString())
    };

    var issuer = _config.GetSection("Jwt:Issuer").Value ?? "Default Issuer";
    var audience = _config.GetSection("Jwt:Audience").Value ?? "Default Audience";

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value ?? "Default Key"));
    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    var descriptor = new SecurityTokenDescriptor
    {
      Issuer = issuer,
      Audience = audience,
      Expires = DateTime.Now.AddDays(2),
      Subject = new ClaimsIdentity(claims),
      SigningCredentials = signingCredentials,
    };

    var token = tokenHandler.CreateToken(descriptor);
    return tokenHandler.WriteToken(token);
  }
}