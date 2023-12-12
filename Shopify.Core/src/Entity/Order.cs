using System.Text.Json.Serialization;

namespace Shopify.Core.src.Entity;

public class Order : BaseEntity
{
  public required Guid CustomerId { get; set; }

  public required Guid AddressId { get; set; }
  public required Status Status { get; set; } = Status.Unpaid;
  public required DateTime OrderTime { get; set; } = DateTime.Now;
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