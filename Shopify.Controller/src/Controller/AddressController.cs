using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Controller.src.Controller;

[Route("api/v1/[controller]es")]
public class AddressController : BaseController<Address, AddressReadDTO, AddressCreateDTO, AddressUpdateDTO>
{
  public AddressController(IAddressService service) : base(service)
  {
  }

  [Authorize(Roles = "Customer")]
  public override async Task<ActionResult<AddressReadDTO>> CreateOneAsync([FromBody] AddressCreateDTO createDTO)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    createDTO.UserId = userId;

    return await base.CreateOneAsync(createDTO);
  }

  [Authorize(Roles = "Customer")]
  public override async Task<ActionResult<AddressReadDTO>> UpdateOneAsync([FromRoute] Guid id, [FromBody] AddressUpdateDTO updateDTO)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    if (userId != updateDTO.UserId)
    {
      throw CustomException.NotAllowed();
    }

    var result = await _service.UpdateOneAsync(id, updateDTO);

    return Ok(result);
  }

  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    var addressEntity = await _service.GetByIdAsync(id);

    if (userId != addressEntity.UserId)
    {
      throw CustomException.NotAllowed();
    }

    return await base.DeleteOneAsync(id);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<IEnumerable<AddressReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    return await base.GetAllAsync(options);
  }

  [Authorize]
  public override async Task<ActionResult<AddressReadDTO>> GetByIdAsync([FromRoute] Guid id)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    var addressEntity = await _service.GetByIdAsync(id);

    if (addressEntity.UserId != userId)
    {
      throw CustomException.NotAllowed();
    }

    return addressEntity;
  }
}