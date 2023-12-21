namespace Shopify.Core.src.Entity;

public class Image : BaseEntity
{
  public required string URL { get; set; }
  public Guid ProductId { get; set; }
}