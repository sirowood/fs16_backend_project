namespace Shopify.Core.src.Entity;

public class Category : BaseEntity
{
  public required string Name { get; set; }
  public required string Image { get; set; }
  public IEnumerable<Product> Products { get; } = new List<Product>();
}