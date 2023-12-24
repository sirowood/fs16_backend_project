namespace Shopify.Service.src.DTO;

public class GetAllResponse<TReadDTO>
{
  public required IEnumerable<TReadDTO> Items { get; set; }
  public int Total { get; set; }
}