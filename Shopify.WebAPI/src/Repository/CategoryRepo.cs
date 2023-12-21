using Microsoft.EntityFrameworkCore;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class CategoryRepo : BaseRepo<Category>, ICategoryRepo
{

  public CategoryRepo(DatabaseContext database) : base(database)
  {
  }

  public async Task<bool> NameIsAvailable(string name)
  {
    var entity = await _data.FirstOrDefaultAsync(entity => entity.Name == name);

    return entity is null;
  }
}