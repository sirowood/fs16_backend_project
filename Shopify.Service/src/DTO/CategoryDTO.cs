namespace Shopify.Service.src.DTO;

public class CategoryReadDTO
{
  public required Guid Id { get; set; }
  public required string Name { get; set; }
  public required string Image { get; set; }
}

public class CategoryCreateDTO
{
  public required string Name { get; set; }
  public required string Image { get; set; }
}

public class CategoryUpdateDTO
{
  public required string Name { get; set; }
  public required string Image { get; set; }
}