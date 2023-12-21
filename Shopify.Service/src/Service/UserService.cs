using AutoMapper;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
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

  private async Task<bool> EmailIsAvailable(string email)
  {
    return await _repo.GetByEmailAsync(email) is null;
  }

  public override async Task<UserReadDTO> CreateOneAsync(UserCreateDTO createDTO)
  {
    var emailAvailable = await EmailIsAvailable(createDTO.Email);

    if (!emailAvailable)
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

  public async Task<bool> UpdatePasswordAsync(Guid userId, ChangePasswords passwords)
  {
    var user = await _repo.GetByIdAsync(userId)
      ?? throw CustomException.NotFound();

    var isPasswordMatch = PasswordService.VerifyPassword(passwords.OriginalPassword, user.Password, user.Salt);

    if (!isPasswordMatch)
    {
      throw CustomException.WrongPassword();
    }

    PasswordService.HashPassword(passwords.NewPassword, out string salt, out string hashedPassword);

    user.Password = hashedPassword;
    user.Salt = salt;
    user.UpdatedAt = DateTime.UtcNow;

    await _repo.UpdateOneAsync(user);

    return true;
  }

  public override async Task<UserReadDTO> UpdateOneAsync(Guid userId, UserUpdateDTO updateDTO)
  {
    var originalEntity = await _repo.GetByIdAsync(userId)
      ?? throw CustomException.NotFound();

    if (updateDTO.Email is not null && updateDTO.Email != originalEntity.Email)
    {
      var emailAvailable = await EmailIsAvailable(updateDTO.Email);

      if (!emailAvailable)
      {
        throw CustomException.EmailIsNotAvailable();
      }
    }

    var updatedEntity = _mapper.Map(updateDTO, originalEntity);
    updatedEntity.UpdatedAt = DateTime.UtcNow;

    var result = await _repo.UpdateOneAsync(updatedEntity);

    return _mapper.Map<User, UserReadDTO>(result);
  }
}