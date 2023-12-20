using System.Text.Json.Serialization;

namespace Shopify.Core.src.Entity;

public class Order : BaseEntity
{
  public required Guid UserId { get; set; }
  public required Guid AddressId { get; set; }
  public required Status Status { get; set; } = Status.Unpaid;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
  Unpaid,
  Paid,
  Delivering,
  Delivered,
  Return,
  Cancel
}