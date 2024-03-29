namespace Shopify.Core.src.Entity;

public class OrderDetail : TimeStamp
{
  public required Guid OrderId { get; set; }
  public required Guid ProductId { get; set; }
  public required Product Product { get; set; }
  public required int Quantity { get; set; }
  public required decimal PriceAtPurchase { get; set; }
}