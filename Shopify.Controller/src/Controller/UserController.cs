using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]s")]
public class UserController : ControllerBase
{
  private readonly IUserService _userService;

  public UserController(IUserService userService)
  {
    _userService = userService;
  }

  [AllowAnonymous]
  [HttpPost()]
  public ActionResult<UserReadDTO> CreateOne([FromBody] UserCreateDTO createDTO)
  {
    var createdUser = _userService.CreateOne(createDTO);
    return CreatedAtAction(nameof(CreateOne), createdUser);
  }
}