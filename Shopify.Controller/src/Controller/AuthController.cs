using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
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

    return Ok(new { token = result });
  }

  [AllowAnonymous]
  [HttpPost("register")]
  public async Task<ActionResult<bool>> RegisterAsync([FromBody] UserRegisterDTO createDTO)
  {
    var result = await _authService.RegisterAsync(createDTO);

    return Ok(result);
  }

  [Authorize]
  [HttpGet("profile")]
  public async Task<ActionResult<UserReadDTO>> GetProfile()
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    var result = await _authService.GetProfileAsync(userId);

    return Ok(result);
  }
}