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
  public virtual async Task<T> CreateOneAsync(T createObject)
  {
    _data.Add(createObject);
    await _databaseContext.SaveChangesAsync();

    return createObject;
  }

  public async Task<bool> DeleteOneAsync(T deleteObject)
  {
    _data.Remove(deleteObject);
    await _databaseContext.SaveChangesAsync();

    return true;
  }

  public virtual async Task<IEnumerable<T>> GetAllAsync(GetAllOptions options)
  {
    var entities = await _data
      .AsNoTracking()
      .OrderBy(entity => entity.Id)
      .Skip(options.Offset)
      .Take(options.Limit)
      .ToArrayAsync();

    return entities;
  }

  public virtual async Task<int> GetTotal(GetAllOptions options)
  {
    var total = await _data.AsNoTracking().CountAsync();

    return total;
  }

  public virtual async Task<T?> GetByIdAsync(Guid id)
  {
    var result = await _data.FindAsync(id);

    return result;
  }

  public async virtual Task<T> UpdateOneAsync(T updatedEntity)
  {
    _data.Update(updatedEntity);
    await _databaseContext.SaveChangesAsync();

    return updatedEntity;
  }
}