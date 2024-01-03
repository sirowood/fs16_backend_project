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
  private readonly IAuthorizationService _authorizationService;
  public AddressController(IAddressService service, IAuthorizationService authorizationService) : base(service)
  {
    _authorizationService = authorizationService;
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
    var address = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Address not found.");

    var authorizationResult = await _authorizationService
      .AuthorizeAsync(User, address, "AddressOwner");

    if (!authorizationResult.Succeeded)
    {
      throw CustomException.NotAllowed("You're not the address owner.");
    }

    var result = await _service.UpdateOneAsync(id, updateDTO);

    return Ok(result);
  }

  [Authorize(Roles = "Customer")]
  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    var address = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Address not found.");

    var authorizationResult = await _authorizationService
      .AuthorizeAsync(User, address, "AddressOwner");

    if (!authorizationResult.Succeeded)
    {
      throw CustomException.NotAllowed("You're not the address owner.");
    }

    return await base.DeleteOneAsync(id);
  }

  [NonAction]
  public override async Task<ActionResult<GetAllResponse<AddressReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    return await base.GetAllAsync(options);
  }

  [NonAction]
  public override async Task<ActionResult<AddressReadDTO>> GetByIdAsync([FromRoute] Guid id)
  {
    return await _service.GetByIdAsync(id);
  }
}