using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class AuthService : IAuthService
{
  private readonly IUserRepo _repo;
  private readonly ITokenService _tokenService;
  protected readonly IMapper _mapper;

  public AuthService(IUserRepo repo, ITokenService tokenService, IMapper mapper)
  {
    _repo = repo;
    _tokenService = tokenService;
    _mapper = mapper;
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

  private async Task CheckEmailAvailability(string email)
  {
    var emailAvailable = await _repo.GetByEmailAsync(email) is null;

    if (!emailAvailable)
    {
      throw CustomException.NotAvailable("Email is not available.");
    }
  }

  public async Task<bool> RegisterAsync(UserCreateDTO createDTO)
  {
    await CheckEmailAvailability(createDTO.Email);

    var user = _mapper.Map<UserCreateDTO, User>(createDTO);

    PasswordService.HashPassword(createDTO.Password, out string salt, out string hashedPassword);

    user.Password = hashedPassword;
    user.Salt = salt;

    await _repo.CreateOneAsync(user);
    return true;
  }
}