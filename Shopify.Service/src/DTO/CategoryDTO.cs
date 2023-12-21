using Shopify.Core.src.Entity;

namespace Shopify.Service.src.DTO;

public class CategoryReadDTO : BaseEntity
{
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