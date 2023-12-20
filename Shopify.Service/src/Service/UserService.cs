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

  public override async Task<UserReadDTO> CreateOneAsync(UserCreateDTO createDTO)
  {
    if (await _repo.GetByEmailAsync(createDTO.Email) is not null)
    {
      throw CustomException.EmailIsNotAvailable();
    }

    var user = _mapper.Map<UserCreateDTO, User>(createDTO);

    PasswordService.HashPassword(createDTO.Password, out string salt, out string hashedPassword);

    user.Password = hashedPassword;
    user.Salt = salt;

    var createdUser = await _repo.CreateOneAsync(user);

    return _mapper.Map<User, UserReadDTO>(createdUser);
  }

  public async Task<UserReadDTO> GetByEmailAsync(string email)
  {
    var result = await _repo.GetByEmailAsync(email)
      ?? throw CustomException.NotFound("No such user.");

    return _mapper.Map<User, UserReadDTO>(result);
  }
}