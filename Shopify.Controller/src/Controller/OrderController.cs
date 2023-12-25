using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Shared;

namespace Shopify.Controller.src.Controller;

[Route("api/v1/[controller]s")]
public class OrderController : BaseController<Order, OrderReadDTO, OrderCreateDTO, OrderUpdateDTO>
{
  private readonly IAuthorizationService _authorizationService;
  private readonly new IOrderService _service;

  public OrderController(IOrderService service, IAuthorizationService authorizationService) : base(service)
  {
    _service = service;
    _authorizationService = authorizationService;
  }

  [Authorize(Roles = "Customer")]
  public override async Task<ActionResult<OrderReadDTO>> CreateOneAsync([FromBody] OrderCreateDTO createDTO)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    createDTO.UserId = userId;

    return await base.CreateOneAsync(createDTO);
  }

  [Authorize(Roles = "Admin")]
  public override Task<ActionResult<GetAllResponse<OrderReadDTO>>> GetAllAsync([FromQuery] GetAllOptions options)
  {
    return base.GetAllAsync(options);
  }

  [Authorize]
  public override async Task<ActionResult<OrderReadDTO>> GetByIdAsync([FromRoute] Guid id)
  {
    var order = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Order not found.");

    var authorizationResult = await _authorizationService
      .AuthorizeAsync(User, order, "OrderOwnerOrAdmin");

    if (!authorizationResult.Succeeded)
    {
      throw CustomException.NotAllowed("You're not the order owner or admin.");
    }

    return await base.GetByIdAsync(id);
  }

  [Authorize]
  [HttpGet("users/{id:guid}")]
  public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetUserOrders([FromRoute] Guid id)
  {
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
    var userId = Guid.Parse(userIdClaim!.Value);

    var userRole = HttpContext.User
      .FindFirst(ClaimTypes.Role)!.Value;

    if (userRole != "Admin" && userId != id)
    {
      throw CustomException.NotAllowed("Only admin or owner could get the orders.");
    }

    var orders = await _service.GetUserOrders(id);

    return Ok(orders);
  }

  // No one could delete order in this application design
  [NonAction]
  public override async Task<ActionResult<bool>> DeleteOneAsync([FromRoute] Guid id)
  {
    var order = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Order not found.");

    if (order.Status != Status.Unpaid)
    {
      throw CustomException.NotAllowed("Only unpaid order can be deleted.");
    }

    var authorizationResult = await _authorizationService
      .AuthorizeAsync(User, order, "OrderOwner");

    if (!authorizationResult.Succeeded)
    {
      throw CustomException.NotAllowed("You're not the order owner.");
    }

    return await base.DeleteOneAsync(id);
  }

  [NonAction]
  public override async Task<ActionResult<OrderReadDTO>> UpdateOneAsync([FromRoute] Guid id, [FromBody] OrderUpdateDTO updateDTO)
  {
    return await base.UpdateOneAsync(id, updateDTO);
  }

  [Authorize(Roles = "Customer")]
  [HttpPatch("{id:guid}/pay")]
  public async Task<ActionResult<bool>> PayOrderAsync([FromRoute] Guid id)
  {
    var order = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Order not found.");

    if (order.Status != Status.Unpaid)
    {
      throw CustomException.NotAllowed("Only unpaid order can be paid.");
    }

    var authorizationResult = await _authorizationService
      .AuthorizeAsync(User, order, "OrderOwner");

    if (!authorizationResult.Succeeded)
    {
      throw CustomException.NotAllowed("You're not the order owner.");
    }

    await base.UpdateOneAsync(id, new OrderUpdateDTO { Status = Status.Paid });

    return Ok(true);
  }

  [Authorize(Roles = "Customer")]
  [HttpPatch("{id:guid}/cancel")]
  public async Task<ActionResult<bool>> CancelOrderAsync([FromRoute] Guid id)
  {
    var order = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Order not found.");

    if (order.Status != Status.Unpaid && order.Status != Status.Paid)
    {
      throw CustomException.NotAllowed("Only unpaid or paid order can be cancelled.");
    }

    var authorizationResult = await _authorizationService
      .AuthorizeAsync(User, order, "OrderOwner");

    if (!authorizationResult.Succeeded)
    {
      throw CustomException.NotAllowed("You're not the order owner.");
    }

    await base.UpdateOneAsync(id, new OrderUpdateDTO { Status = Status.Cancel });

    return Ok(true);
  }

  [Authorize(Roles = "Customer")]
  [HttpPatch("{id:guid}/return")]
  public async Task<ActionResult<bool>> ReturnOrderAsync([FromRoute] Guid id)
  {
    var order = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Order not found.");

    if (order.Status != Status.Delivered)
    {
      throw CustomException.NotAllowed("Only delivered order can be returned.");
    }

    var authorizationResult = await _authorizationService
      .AuthorizeAsync(User, order, "OrderOwner");

    if (!authorizationResult.Succeeded)
    {
      throw CustomException.NotAllowed("You're not the order owner.");
    }

    await base.UpdateOneAsync(id, new OrderUpdateDTO { Status = Status.Return });

    return Ok(true);
  }

  [Authorize(Roles = "Admin")]
  [HttpPatch("{id:guid}/delivering")]
  public async Task<ActionResult<bool>> DeliverOrderAsync([FromRoute] Guid id)
  {
    var order = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Order not found.");

    if (order.Status != Status.Paid)
    {
      throw CustomException.NotAllowed("Only paid order can be marked as delivering.");
    }

    await base.UpdateOneAsync(id, new OrderUpdateDTO { Status = Status.Delivering });

    return Ok(true);
  }

  [Authorize(Roles = "Admin")]
  [HttpPatch("{id:guid}/delivered")]
  public async Task<ActionResult<bool>> DeliveredOrderAsync([FromRoute] Guid id)
  {
    var order = await _service.GetByIdAsync(id)
      ?? throw CustomException.NotFound("Order not found.");

    if (order.Status != Status.Delivering)
    {
      throw CustomException.NotAllowed("Only delivering order can be delivered.");
    }

    await base.UpdateOneAsync(id, new OrderUpdateDTO { Status = Status.Delivered });

    return Ok(true);
  }
}