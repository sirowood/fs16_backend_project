using Shopify.Core.src.Entity;

namespace Shopify.Core.src.Abstraction;

public interface IUserRepo : IBaseRepo<User>
{
  User? GetByEmail(string email);
  User? Login(string email, string password);
  string GenerateToken(User user);
}