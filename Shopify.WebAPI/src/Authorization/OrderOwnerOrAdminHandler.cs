using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;

namespace Shopify.WebAPI.src.Authorization;

public class OrderOwnerOrAdminHandler : AuthorizationHandler<OrderOwnerOrAdminRequirement, OrderReadDTO>
{
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderOwnerOrAdminRequirement requirement, OrderReadDTO order)
  {
    var userIdClaim = context
      .User
      .FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim == null)
    {
      return Task.CompletedTask;
    }

    var userRole = context
      .User
      .FindFirst(ClaimTypes.Role)!.Value;

    var userId = Guid.Parse(userIdClaim.Value);

    if (userId == order.UserId || userRole == Role.Admin.ToString())
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}

public class OrderOwnerOrAdminRequirement : IAuthorizationRequirement
{

}