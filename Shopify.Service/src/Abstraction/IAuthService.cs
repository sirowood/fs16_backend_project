using Shopify.Core.src.Shared;

namespace Shopify.Service.src.Abstraction;

public interface IAuthService
{
  string Login(Credentials credentials);
}