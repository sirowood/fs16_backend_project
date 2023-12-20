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
  private readonly IUserService _userService;

  public UserController(IUserService userService) : base(userService)
  {
    _userService = userService;
  }

  [AllowAnonymous]
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
}