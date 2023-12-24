using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Abstraction;

public interface IOrderService : IBaseService<Order, OrderReadDTO, OrderCreateDTO, OrderUpdateDTO>
{
  Task<IEnumerable<OrderReadDTO>> GetUserOrders(Guid id);
}