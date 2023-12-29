namespace Shopify.Service.src.DTO;

public class GetAllResponse<TReadDTO>
{
  public required IEnumerable<TReadDTO> Items { get; set; }
  public required int Pages { get; set; }
  public required int Total { get; set; }
}