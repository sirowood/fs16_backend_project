using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;

namespace Shopify.Core.src.Abstraction;

public interface IBaseRepo<T> where T : BaseEntity
{
  IEnumerable<T> GetAll(GetAllOptions options);
  T GetById(Guid id);
  T CreateOne(T newObject);
  T UpdateOne(T updateObject);
  bool DeleteOne(Guid id);
}