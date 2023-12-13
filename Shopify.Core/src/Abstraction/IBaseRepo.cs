using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;

namespace Shopify.Core.src.Abstraction;

public interface IBaseRepo<T> where T : BaseEntity
{
  IEnumerable<T> GetAll(GetAllOptions options);
  T GetById(Guid id);
  T CreateOne(T createObject);
  T UpdateOne(T updateObject);
  bool DeleteOne(Guid id);
}