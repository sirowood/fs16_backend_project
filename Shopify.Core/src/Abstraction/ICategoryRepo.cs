using Shopify.Core.src.Entity;

namespace Shopify.Core.src.Abstraction;

public interface ICategoryRepo : IBaseRepo<Category>
{
  Task<bool> NameIsAvailable(string name);
}