using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

using Shopify.Service.src.DTO;

namespace Shopify.WebAPI.src.Authorization;

public class OrderOwnerHandler : AuthorizationHandler<OrderOwnerRequirement, OrderReadDTO>
{
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderOwnerRequirement requirement, OrderReadDTO order)
  {
    var userIdClaim = context
      .User
      .FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim == null)
    {
      return Task.CompletedTask;
    }

    var userId = Guid.Parse(userIdClaim.Value);

    if (userId == order.UserId)
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}

public class OrderOwnerRequirement : IAuthorizationRequirement
{

}