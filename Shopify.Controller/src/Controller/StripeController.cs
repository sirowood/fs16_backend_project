using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

using Shopify.Core.src.Entity;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;

namespace Shopify.Controller.src.Controller;

[Route("api/v1/webhook")]
[ApiController]
public class WebhookController : ControllerBase
{
  private readonly IOrderService _orderService;
  private readonly IConfiguration _configuration;

  public WebhookController(IOrderService orderService, IConfiguration configuration)
  {
    _orderService = orderService;
    _configuration = configuration;
  }

  [HttpPost]
  public async Task<IActionResult> IndexAsync()
  {
    Console.WriteLine(_configuration["Stripe:EndpointSecret"]);
    try
    {
      var json = await new StreamReader(HttpContext.Request.Body)
        .ReadToEndAsync();

      var stripeEvent = EventUtility.ConstructEvent(
        json,
        Request.Headers["Stripe-Signature"],
        _configuration["Stripe:EndpointSecret"]
      );

      if (stripeEvent.Type == Events.CheckoutSessionCompleted)
      {
        var objData = (Session)stripeEvent.Data.Object;

        var orderIdString = objData?.Metadata["orderId"];

        if (orderIdString is not null)
        {
          var orderId = Guid.Parse(orderIdString);
          await _orderService.UpdateOneAsync(orderId, new OrderUpdateDTO { Status = Status.Paid });
        }
      }

      return Ok();
    }
    catch (StripeException e)
    {
      return BadRequest(e.Message);
    }
  }
}