using Shopify.Core.src.Entity;

namespace Shopify.Service.src.DTO;

public class UserReadDTO : BaseEntity
{
  public Role Role { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
  public required IEnumerable<Address> Addresses { get; set; }
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