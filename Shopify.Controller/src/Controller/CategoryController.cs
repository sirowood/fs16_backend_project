using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Controller.src.Controller;

[Route("api/v1/categories")]
public class CategoryController : BaseController<Category, CategoryReadDTO, CategoryCreateDTO, CategoryUpdateDTO>
{
  public CategoryController(ICategoryService service) : base(service)
  {
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<CategoryReadDTO>> CreateOneAsync([FromBody] CategoryCreateDTO createDTO)
  {
    return await base.CreateOneAsync(createDTO);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    return await base.DeleteOneAsync(id);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<CategoryReadDTO>> UpdateOneAsync([FromRoute] Guid id, [FromBody] CategoryUpdateDTO updateDTO)
  {
    return await base.UpdateOneAsync(id, updateDTO);
  }
}