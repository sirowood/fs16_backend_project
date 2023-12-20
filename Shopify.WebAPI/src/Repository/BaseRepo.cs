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
  public T CreateOne(T createObject)
  {
    _data.Add(createObject);
    _databaseContext.SaveChanges();

    return createObject;
  }

  public bool DeleteOne(Guid id)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<T> GetAll(GetAllOptions options)
  {
    return _data
      .Skip(options.Offset)
      .Take(options.Limit);
  }

  public T GetById(Guid id)
  {
    throw new NotImplementedException();
  }

  public T UpdateOne(T updateObject)
  {
    throw new NotImplementedException();
  }
}