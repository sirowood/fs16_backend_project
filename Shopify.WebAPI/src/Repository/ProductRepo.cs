using Microsoft.EntityFrameworkCore;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class ProductRepo : BaseRepo<Product>, IProductRepo
{

  public ProductRepo(DatabaseContext database) : base(database)
  {
  }

  public override async Task<Product> CreateOneAsync(Product createObject)
  {
    _data.Add(createObject);
    await _databaseContext.SaveChangesAsync();

    var category = await _databaseContext
      .Categories
      .FirstOrDefaultAsync(category => category.Id == createObject.CategoryId);

    createObject.Category = category;

    return createObject;
  }

  public override async Task<IEnumerable<Product>> GetAllAsync(GetAllOptions options)
  {
    var result = await _data
      .Include(e => e.Category)
      .Include(e => e.Images)
      .OrderBy(entity => entity.Id)
      .Skip(options.Offset)
      .Take(options.Limit)
      .ToArrayAsync();

    return result;
  }
  public override async Task<Product?> GetByIdAsync(Guid id)
  {
    var result = await _data
      .Include(e => e.Category)
      .Include(e => e.Images)
      .FirstOrDefaultAsync(e => e.Id == id);

    return result;
  }
}