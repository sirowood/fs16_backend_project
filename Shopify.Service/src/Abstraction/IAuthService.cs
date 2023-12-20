using Shopify.Core.src.Shared;

namespace Shopify.Service.src.Abstraction;

public interface IAuthService
{
  Task<string> LoginAsync(Credentials credentials);
}