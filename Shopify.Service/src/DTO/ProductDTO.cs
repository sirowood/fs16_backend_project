namespace Shopify.Service.src.DTO;

public class ProductReadDTO
{
  public required Guid Id { get; set; }
  public required string Title { get; set; }
  public required string Description { get; set; }
  public required decimal Price { get; set; }
  public required CategoryReadDTO Category { get; set; }
  public IEnumerable<ImageReadDTO> Images { get; } = new List<ImageReadDTO>();
}

public class ProductCreateDTO
{
  public required string Title { get; set; }
  public required string Description { get; set; }
  public required decimal Price { get; set; }
  public required Guid CategoryId { get; set; }
  public required IEnumerable<ImageCreateDTO> Images { get; set; }
}

public class ProductUpdateDTO
{
  public required string Title { get; set; }
  public required string Description { get; set; }
  public required decimal Price { get; set; }
  public required Guid CategoryId { get; set; }
}