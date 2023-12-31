using Shopify.Core.src.Entity;

namespace Shopify.Service.src.DTO;

public class OrderReadDTO
{
  public required Guid Id { get; set; }
  public required Guid UserId { get; set; }
  public required Guid AddressId { get; set; }
  public required UserReadDTO User { get; set; }
  public required AddressReadDTO Address { get; set; }
  public required Status Status { get; set; }
  public IEnumerable<OrderDetailReadDTO> OrderDetails { get; } = new List<OrderDetailReadDTO>();
  public required DateTime CreatedAt { get; set; }
  public required DateTime UpdatedAt { get; set; }
}

public class OrderCreateDTO
{
  public Guid? UserId { get; set; }
  public required Guid AddressId { get; set; }
  public required List<OrderDetailCreateDTO> OrderDetails { get; set; }
}

public class OrderUpdateDTO
{
  public required Status Status { get; set; }
}