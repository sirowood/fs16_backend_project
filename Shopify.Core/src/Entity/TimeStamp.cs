namespace Shopify.Core.src.Entity;

public class TimeStamp
{
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}