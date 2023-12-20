using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]s")]
public class UserController : BaseController<User, UserReadDTO, UserCreateDTO, UserUpdateDTO>
{
  private readonly IUserService _userService;

  public UserController(IUserService userService) : base(userService)
  {
    _userService = userService;
  }

  [AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public override async Task<ActionResult<UserReadDTO>> CreateOneAsync([FromBody] UserCreateDTO createDTO)
  {
    var createdUser = await _userService.CreateOneAsync(createDTO);

    return CreatedAtAction(nameof(CreateOneAsync), createdUser);
  }

  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public override async Task<ActionResult<IEnumerable<UserReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    var result = await _userService.GetAllAsync(options);
    return Ok(result);
  }
}