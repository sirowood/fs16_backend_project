namespace Shopify.Service.src.DTO;


public class ImageReadDTO
{
  public required Guid Id { get; set; }
  public required string URL { get; set; }
}

public class ImageCreateDTO
{
  public Guid? ProductId { get; set; }
  public required string URL { get; set; }
}

public class ImageUpdateDTO
{
}