namespace Shopify.Service.src.DTO;

public class OrderDetailReadDTO
{
  public required Guid ProductId { get; set; }
  public required int Quantity { get; set; }
  public required decimal PriceAtPurchase { get; set; }
}

public class OrderDetailCreateDTO
{
  public required Guid ProductId { get; set; }
  public required int Quantity { get; set; }
  public required decimal PriceAtPurchase { get; set; }
}