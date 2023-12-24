using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

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
  public override async Task<ActionResult<IEnumerable<UserReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    return await base.GetAllAsync(options);
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

    var result = await base.UpdateOneAsync(userId, updateDTO);

    return Ok(result);
  }

  [Authorize]
  [HttpDelete("profile")]
  public async Task<ActionResult<bool>> DeleteOneAsync()
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    var result = await base.DeleteOneAsync(userId);

    return Ok(result);
  }

  [Authorize(Roles = "Admin")]
  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    return await base.DeleteOneAsync(id);
  }
}