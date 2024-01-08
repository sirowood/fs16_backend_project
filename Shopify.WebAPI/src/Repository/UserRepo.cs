using Microsoft.EntityFrameworkCore;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class UserRepo : BaseRepo<User>, IUserRepo
{

  public UserRepo(DatabaseContext database) : base(database)
  {
  }

  public override async Task<IEnumerable<User>> GetAllAsync(GetAllOptions options)
  {
    var result = await _data
      .Include(u => u.Addresses)
      .OrderBy(entity => entity.Id)
      .Skip(options.Offset)
      .Take(options.Limit)
      .ToArrayAsync();

    return result;
  }

  public override async Task<User?> GetByIdAsync(Guid id)
  {
    var result = await _data
      .Include(e => e.Addresses)
      .AsSplitQuery()
      .FirstOrDefaultAsync(e => e.Id == id);

    return result;
  }

  public async Task<User?> GetByEmailAsync(string email)
  {
    var result = await _data.FirstOrDefaultAsync(u => u.Email == email);

    return result;
  }
}