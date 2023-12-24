using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Controller.src.Controller;

public class ImageController : BaseController<Image, ImageReadDTO, ImageCreateDTO, ImageUpdateDTO>
{
  public ImageController(IImageService service) : base(service)
  {
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<ImageReadDTO>> CreateOneAsync([FromBody] ImageCreateDTO createDTO)
  {
    return await base.CreateOneAsync(createDTO);
  }

  [NonAction]
  public override Task<ActionResult<IEnumerable<ImageReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    return base.GetAllAsync(options);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    return await base.DeleteOneAsync(id);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<ImageReadDTO>> UpdateOneAsync([FromRoute] Guid id, [FromBody] ImageUpdateDTO updateDTO)
  {
    return await base.UpdateOneAsync(id, updateDTO);
  }
}