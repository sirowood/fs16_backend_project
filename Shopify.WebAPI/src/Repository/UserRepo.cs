using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class UserRepo : BaseRepo<User>, IUserRepo
{
  private readonly IConfiguration _config;

  public UserRepo(DatabaseContext database, IConfiguration config) : base(database)
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

  public User? GetByEmail(string email)
  {
    return _data.FirstOrDefault(u => u.Email == email);
  }
}