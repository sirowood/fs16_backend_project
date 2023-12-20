using Shopify.Core.src.Entity;

namespace Shopify.Service.src.DTO;

public class UserReadDTO : BaseEntity
{
  public Role Role { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
  public required IEnumerable<AddressReadDTO> Addresses { get; set; } = new List<AddressReadDTO>();
}

public class UserCreateDTO
{
  public Role Role { get; set; } = Role.Customer;
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
}

public class UserUpdateDTO
{
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
}