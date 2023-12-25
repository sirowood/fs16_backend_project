using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

  private static IQueryable<Product> ApplyOrder(IQueryable<Product> query, string? orderBy = "Id", string? direction = "Asc")
  {
    return orderBy?.ToLower() switch
    {
      "title" => direction == "desc"
        ? query.OrderByDescending(entity => entity.Title)
        : query.OrderBy(entity => entity.Title),
      "price" => direction == "desc"
        ? query.OrderByDescending(entity => entity.Price)
        : query.OrderBy(entity => entity.Price),
      _ => query,
    } ?? query;
  }

  public override async Task<IEnumerable<Product>> GetAllAsync(GetAllOptions options)
  {
    var query = _data
        .Include(e => e.Category)
        .Include(e => e.Images)
        .AsQueryable();

    if (options.CategoryId.HasValue)
    {
      query = query.Where(entity => entity.CategoryId == options.CategoryId);
    }

    if (!options.Title.IsNullOrEmpty())
    {
      query = query.Where(entity => entity.Title.Contains(options.Title!));
    }

    if (!options.OrderBy.IsNullOrEmpty())
    {
      query = ApplyOrder(query, options.OrderBy, options.Direction);
    }

    var result = await query
        .Skip(options.Offset)
        .Take(options.Limit)
        .ToArrayAsync();

    return result;
  }

  public override async Task<int> GetTotal(GetAllOptions options)
  {
    return await _data
      .Where(entity => !options.CategoryId.HasValue || entity.CategoryId == options.CategoryId)
      .Where(entity => string.IsNullOrEmpty(options.Title) || entity.Title.Contains(options.Title))
      .CountAsync();
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