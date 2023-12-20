using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class UserRepo : BaseRepo<User>, IUserRepo
{

  public UserRepo(DatabaseContext database) : base(database)
  {
  }

  public User? GetByEmail(string email)
  {
    return _data.FirstOrDefault(u => u.Email == email);
  }
}