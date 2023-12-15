using Shopify.Core.src.Entity;

namespace Shopify.Core.src.Abstraction;

public interface IUserRepo : IBaseRepo<User>
{
  User? GetByEmail(string email);
}