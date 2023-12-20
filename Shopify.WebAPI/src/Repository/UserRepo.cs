using Microsoft.EntityFrameworkCore;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class UserRepo : BaseRepo<User>, IUserRepo
{

  public UserRepo(DatabaseContext database) : base(database)
  {
  }

  public async Task<User?> GetByEmailAsync(string email)
  {
    var result = await _data.FirstOrDefaultAsync(u => u.Email == email);

    return result;
  }
}