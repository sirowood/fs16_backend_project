namespace Shopify.Core.src.Shared;

public class GetAllOptions
{
  public int Limit { get; set; } = 20;
  public int Offset { get; set; } = 0;
  public Guid? CategoryId { get; set; }
  public string? Title { get; set; }
}