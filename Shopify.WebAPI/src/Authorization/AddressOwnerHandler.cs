using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

using Shopify.Service.src.DTO;

namespace Shopify.WebAPI.src.Authorization;

public class AddressOwnerHandler : AuthorizationHandler<AddressOwnerRequirement, AddressReadDTO>
{
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AddressOwnerRequirement requirement, AddressReadDTO address)
  {
    var userIdClaim = context
      .User
      .FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim == null)
    {
      return Task.CompletedTask;
    }

    var userId = Guid.Parse(userIdClaim.Value);

    if (userId == address.UserId)
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}

public class AddressOwnerRequirement : IAuthorizationRequirement
{

}