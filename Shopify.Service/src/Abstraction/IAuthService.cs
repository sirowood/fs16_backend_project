using Shopify.Core.src.Shared;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Abstraction;

public interface IAuthService
{
  Task<string> LoginAsync(Credentials credentials);
  Task<bool> RegisterAsync(UserCreateDTO createDTO);
}