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
  public override ActionResult<UserReadDTO> CreateOne([FromBody] UserCreateDTO createDTO)
  {
    var createdUser = _userService.CreateOne(createDTO);
    return CreatedAtAction(nameof(CreateOne), createdUser);
  }

  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public override ActionResult<IEnumerable<UserReadDTO>> GetAll([FromQuery] GetAllOptions options)
  {
    return Ok(_userService.GetAll(options));
  }
}