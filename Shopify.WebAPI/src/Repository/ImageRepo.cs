using Microsoft.EntityFrameworkCore;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.WebAPI.src.Database;

namespace Shopify.WebAPI.src.Repository;

public class ImageRepo : BaseRepo<Image>, IImageRepo
{

  public ImageRepo(DatabaseContext database) : base(database)
  {
  }

  public async Task RemoveImagesByProductId(Guid id)
  {
    await _data
      .Where(img => img.ProductId == id)
      .ExecuteDeleteAsync();

    await _databaseContext.SaveChangesAsync();
  }
}