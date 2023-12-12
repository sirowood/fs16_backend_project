using System.Text.Json.Serialization;

namespace Shopify.Core.src.Entity;

public class User : BaseEntity
{
  public Role Role { get; set; } = Role.Customer;
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
  Admin,
  Customer
}