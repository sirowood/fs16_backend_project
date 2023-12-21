using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Controller.src.Controller;

[Route("api/v1/[controller]s")]
public class ProductController : BaseController<Product, ProductReadDTO, ProductCreateDTO, ProductUpdateDTO>
{
  public ProductController(IProductService service) : base(service)
  {
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<ProductReadDTO>> CreateOneAsync([FromBody] ProductCreateDTO createDTO)
  {
    return await base.CreateOneAsync(createDTO);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    return await base.DeleteOneAsync(id);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<ProductReadDTO>> UpdateOneAsync([FromRoute] Guid id, [FromBody] ProductUpdateDTO updateDTO)
  {
    return await base.UpdateOneAsync(id, updateDTO);
  }
}