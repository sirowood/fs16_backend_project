using Shopify.Core.src.Entity;

namespace Shopify.Core.src.Abstraction;

public interface IOrderRepo : IBaseRepo<Order>
{
  Task<IEnumerable<Order>> GetUserOrders(Guid id);
}