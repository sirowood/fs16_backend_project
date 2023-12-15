using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public ActionResult<UserReadDTO> CreateOne([FromBody] UserCreateDTO createDTO)
  {
    var createdUser = _userService.CreateOne(createDTO);
    return CreatedAtAction(nameof(CreateOne), createdUser);
  }

  [HttpPost("login")]
  public ActionResult<string> Login([FromBody] LoginDTO loginDTO)
  {
    Console.WriteLine(loginDTO.ToString());
    return _userService.Login(loginDTO);
  }
}