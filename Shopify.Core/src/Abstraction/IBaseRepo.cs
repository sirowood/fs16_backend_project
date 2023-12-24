using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;

namespace Shopify.Core.src.Abstraction;

public interface IBaseRepo<T> where T : BaseEntity
{
  Task<IEnumerable<T>> GetAllAsync(GetAllOptions options);
  Task<int> GetTotal();
  Task<T?> GetByIdAsync(Guid id);
  Task<T> CreateOneAsync(T createObject);
  Task<T> UpdateOneAsync(T updateObject);
  Task<bool> DeleteOneAsync(T deleteObject);
}