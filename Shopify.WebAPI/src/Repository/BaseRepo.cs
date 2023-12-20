using Microsoft.EntityFrameworkCore;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
{
  protected readonly DbSet<T> _data;
  protected readonly DatabaseContext _databaseContext;

  public BaseRepo(DatabaseContext databaseContext)
  {
    _databaseContext = databaseContext;
    _data = _databaseContext.Set<T>();
  }
  public async Task<T> CreateOneAsync(T createObject)
  {
    _data.Add(createObject);
    await _databaseContext.SaveChangesAsync();

    return createObject;
  }

  public async Task<bool> DeleteOneAsync(Guid id)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<T>> GetAllAsync(GetAllOptions options)
  {
    var result = await _data
      .Skip(options.Offset)
      .Take(options.Limit)
      .ToArrayAsync();

    return result;
  }

  public async Task<T> GetByIdAsync(Guid id)
  {
    throw new NotImplementedException();
  }

  public async Task<T> UpdateOneAsync(T updateObject)
  {
    throw new NotImplementedException();
  }
}