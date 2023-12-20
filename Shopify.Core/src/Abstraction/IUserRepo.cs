using Shopify.Core.src.Entity;

namespace Shopify.Core.src.Abstraction;

public interface IUserRepo : IBaseRepo<User>
{
  Task<User?> GetByEmailAsync(string email);
}