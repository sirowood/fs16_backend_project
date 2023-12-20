using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class AuthService : IAuthService
{
  private readonly IUserRepo _repo;
  private readonly ITokenService _tokenService;

  public AuthService(IUserRepo repo, ITokenService tokenService)
  {
    _repo = repo;
    _tokenService = tokenService;
  }

  public async Task<string> LoginAsync(Credentials credentials)
  {
    var user = await _repo.GetByEmailAsync(credentials.Email)
      ?? throw CustomException.LoginFailed();

    var isPasswordMatch = PasswordService.VerifyPassword(credentials.Password, user.Password, user.Salt);

    if (!isPasswordMatch)
    {
      throw CustomException.LoginFailed();
    }

    var token = _tokenService.GenerateToken(user);

    return token;
  }
}