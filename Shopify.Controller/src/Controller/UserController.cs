using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Controller.src.Controller;

public class UserController : BaseController<User, UserReadDTO, UserCreateDTO, UserUpdateDTO>
{
  private new readonly IUserService _service;
  public UserController(IUserService service) : base(service)
  {
    _service = service;
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<UserReadDTO>> CreateOneAsync([FromBody] UserCreateDTO createDTO)
  {
    return await base.CreateOneAsync(createDTO);
  }

  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public override async Task<ActionResult<GetAllResponse<UserReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    return await base.GetAllAsync(options);
  }

  [Authorize]
  public override async Task<ActionResult<UserReadDTO>> GetByIdAsync([FromRoute] Guid id)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    var userRole = HttpContext.User
      .FindFirst(ClaimTypes.Role)!.Value;

    if (userId != id && userRole != Role.Admin.ToString())
    {
      throw CustomException.NotAllowed();
    }

    return await base.GetByIdAsync(id);
  }

  [Authorize()]
  [HttpPost("change-password")]
  public async Task<ActionResult<bool>> UpdatePasswordAsync([FromBody] ChangePasswords passwords)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    var result = await _service.UpdatePasswordAsync(userId, passwords);

    return Ok(result);
  }

  [Authorize()]
  [HttpPatch("profile")]
  public async Task<ActionResult<UserReadDTO>> UpdateOneAsync([FromBody] UserUpdateDTO updateDTO)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    return await base.UpdateOneAsync(userId, updateDTO);
  }

  [Authorize]
  [HttpDelete("profile")]
  public async Task<ActionResult<bool>> DeleteOneAsync()
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    return await base.DeleteOneAsync(userId);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    return await base.DeleteOneAsync(id);
  }
}