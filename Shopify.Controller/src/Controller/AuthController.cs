using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("login")]
  public async Task<ActionResult<string>> LoginAsync([FromBody] Credentials credentials)
  {
    var result = await _authService.LoginAsync(credentials);
    return result;
  }

  [AllowAnonymous]
  [HttpPost("register")]
  public async Task<ActionResult<bool>> RegisterAsync([FromBody] UserCreateDTO createDTO)
  {
    // Force the role to be Customer for security
    createDTO.Role = Role.Customer;

    var result = await _authService.RegisterAsync(createDTO);

    return Ok(result);
  }
}