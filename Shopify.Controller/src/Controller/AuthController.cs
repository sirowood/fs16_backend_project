using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;

namespace Shopify.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController
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
}