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

  private static IQueryable<Product> ApplyOrder(IQueryable<Product> query, string? orderBy = "CreatedAt", string direction = "Desc")
  {
    return orderBy?.ToLower() switch
    {
      "title" => direction.ToLower() == "desc"
        ? query.OrderByDescending(entity => entity.Title)
        : query.OrderBy(entity => entity.Title),
      "price" => direction.ToLower() == "desc"
        ? query.OrderByDescending(entity => entity.Price)
        : query.OrderBy(entity => entity.Price),
      "category" => direction.ToLower() == "desc"
        ? query.OrderByDescending(entity => entity.CategoryId)
        : query.OrderBy(entity => entity.CategoryId),
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
      query = query.Where(entity => EF.Functions.ILike(entity.Title, $"%{options.Title}%"));
    }

    if (!options.OrderBy.IsNullOrEmpty())
    {
      query = ApplyOrder(query, options.OrderBy, options.Direction ?? "Desc");
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
      .Where(entity => string.IsNullOrEmpty(options.Title) || EF.Functions.ILike(entity.Title, $"%{options.Title}%"))
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

  public override Task<Product> UpdateOneAsync(Product updatedEntity)
  {
    return base.UpdateOneAsync(updatedEntity);
  }
}