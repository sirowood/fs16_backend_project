using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Abstraction;

public interface IUserService : IBaseService<User, UserReadDTO, UserCreateDTO, UserUpdateDTO>
{
  UserReadDTO GetByEmail(string email);
}