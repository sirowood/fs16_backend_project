using Shopify.Core.src.Entity;

namespace Shopify.Service.src.DTO;

public class AddressReadDTO
{
  public required Guid Id { get; set; }
  public required string Street { get; set; }
  public required string PostCode { get; set; }
  public required string City { get; set; }
  public required string Country { get; set; }
  public required Guid UserId { get; set; }
}

public class AddressCreateDTO
{
  public required string Street { get; set; }
  public required string PostCode { get; set; }
  public required string City { get; set; }
  public required string Country { get; set; }
  public Guid? UserId { get; set; }
}

public class AddressUpdateDTO
{
  public required string Street { get; set; }
  public required string PostCode { get; set; }
  public required string City { get; set; }
  public required string Country { get; set; }
}