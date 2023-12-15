using AutoMapper;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Service.src.Service;

public class UserService : IUserService
{
  private readonly IUserRepo _userRepo;
  private readonly IMapper _mapper;

  public UserService(IUserRepo userRepo, IMapper mapper)
  {
    _userRepo = userRepo;
    _mapper = mapper;
  }

  public UserReadDTO CreateOne(UserCreateDTO createDTO)
  {
    if (_userRepo.GetByEmail(createDTO.Email) is not null)
    {
      throw CustomException.EmailIsNotAvailable();
    }

    var user = _mapper.Map<UserCreateDTO, User>(createDTO);
    var createdUser = _userRepo.CreateOne(user);
    return _mapper.Map<User, UserReadDTO>(createdUser);
  }

  public bool DeleteOne(Guid id)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<UserReadDTO> GetAll(GetAllOptions options)
  {
    throw new NotImplementedException();
  }

  public UserReadDTO GetByEmail(string email)
  {
    var result = _userRepo.GetByEmail(email)
      ?? throw CustomException.NotFound("No such user.");

    return _mapper.Map<User, UserReadDTO>(result);
  }

  public UserReadDTO GetById(Guid id)
  {
    throw new NotImplementedException();
  }

  public string Login(LoginDTO loginDTO)
  {
    var user = _userRepo.Login(loginDTO.Email, loginDTO.Password)
      ?? throw CustomException.LoginFailed();

    return _userRepo.GenerateToken(user);
  }

  public UserReadDTO UpdateOne(UserUpdateDTO updateDTO)
  {
    throw new NotImplementedException();
  }
}