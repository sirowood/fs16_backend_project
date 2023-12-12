namespace Shopify.Core.src.Entity;

public class Product : BaseEntity
{
  public required string Title { get; set; }
  public required string Description { get; set; }
  public required decimal Price { get; set; }
  public required Guid CategoryId { get; set; }
}