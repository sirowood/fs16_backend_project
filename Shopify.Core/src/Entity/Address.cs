namespace Shopify.Core.src.Entity;

public class Address : BaseEntity
{
  public required string Street { get; set; }
  public required string PostCode { get; set; }
  public required string City { get; set; }
  public required string Country { get; set; }
  public required Guid UserId { get; set; }
}