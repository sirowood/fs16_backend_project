using Shopify.Core.src.Entity;

namespace Shopify.Service.src.Abstraction;

public interface ITokenService
{
  string GenerateToken(User user);
}