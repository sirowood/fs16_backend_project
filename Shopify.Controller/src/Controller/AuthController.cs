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
  public ActionResult<string> Login([FromBody] Credentials credentials)
  {
    return _authService.Login(credentials);
  }
}