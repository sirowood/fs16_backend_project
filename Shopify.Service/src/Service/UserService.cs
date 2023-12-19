using AutoMapper;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class UserService : BaseService<User, UserReadDTO, UserCreateDTO, UserUpdateDTO>, IUserService
{
  private new readonly IUserRepo _repo;
  public UserService(IUserRepo repo, IMapper mapper) : base(repo, mapper)
  {
    _repo = repo;
  }

  public override UserReadDTO CreateOne(UserCreateDTO createDTO)
  {
    if (_repo.GetByEmail(createDTO.Email) is not null)
    {
      throw CustomException.EmailIsNotAvailable();
    }

    var user = _mapper.Map<UserCreateDTO, User>(createDTO);

    PasswordService.HashPassword(createDTO.Password, out string salt, out string hashedPassword);

    user.Password = hashedPassword;
    user.Salt = salt;

    var createdUser = _repo.CreateOne(user);

    return _mapper.Map<User, UserReadDTO>(createdUser);
  }

  public UserReadDTO GetByEmail(string email)
  {
    var result = _repo.GetByEmail(email)
      ?? throw CustomException.NotFound("No such user.");

    return _mapper.Map<User, UserReadDTO>(result);
  }

  public string Login(LoginDTO loginDTO)
  {
    var user = _repo.GetByEmail(loginDTO.Email)
      ?? throw CustomException.LoginFailed();

    var isPasswordMatch = PasswordService.VerifyPassword(loginDTO.Password, user.Password, user.Salt);

    if (!isPasswordMatch)
    {
      throw CustomException.LoginFailed();
    }

    return _repo.GenerateToken(user);
  }
}