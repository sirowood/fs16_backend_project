using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class OrderRepo : BaseRepo<Order>, IOrderRepo
{

  public OrderRepo(DatabaseContext database) : base(database)
  {
  }
}