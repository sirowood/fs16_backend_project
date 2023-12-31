using Microsoft.EntityFrameworkCore;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class OrderRepo : BaseRepo<Order>, IOrderRepo
{
  public OrderRepo(DatabaseContext database) : base(database)
  {
  }

  public override async Task<Order?> GetByIdAsync(Guid id)
  {
    return await _data
      .Include(e => e.User)
      .Include(e => e.Address)
      .Include(e => e.OrderDetails)
        .ThenInclude(e => e.Product.Images)
      .FirstOrDefaultAsync(e => e.Id == id);
  }

  public override async Task<IEnumerable<Order>> GetAllAsync(GetAllOptions options)
  {
    var result = await _data
      .Include(u => u.OrderDetails)
        .ThenInclude(e => e.Product.Images)
      .Include(e => e.User)
      .Include(e => e.Address)
      .OrderBy(entity => entity.Id)
      .Skip(options.Offset)
      .Take(options.Limit)
      .AsSplitQuery()
      .ToArrayAsync();

    return result;
  }

  public async Task<IEnumerable<Order>> GetUserOrders(Guid id)
  {
    var result = await _data
      .Where(e => e.UserId == id)
      .Include(u => u.OrderDetails)
        .ThenInclude(e => e.Product.Images)
      .OrderBy(entity => entity.Id)
      .ToArrayAsync();

    return result;
  }
}