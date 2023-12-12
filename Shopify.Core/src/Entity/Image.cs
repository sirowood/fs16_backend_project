namespace Shopify.Core.src.Entity;

public class Image : BaseEntity
{
  public required string Format { get; set; }
  public required string Name { get; set; }
  public required byte[] Data { get; set; }
  public Guid ProductId { get; set; }
}