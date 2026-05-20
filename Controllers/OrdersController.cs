using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Interfaces;
using FulfillmentCenter.Services.UpdateOrderStatus;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private OrderHandlerFactory _orderHandlerFactory = new OrderHandlerFactory();
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        [FromBody] RequestOrderDto? orderDto)
    {
        if(idempotencyKey == null)return BadRequest("missing Idempotency-Key");
        if (orderDto != null)
        {
            await orderService.CreateOrder(orderDto, idempotencyKey);
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeOrderStatus([FromRoute] Guid id,[FromQuery] OrderStatus status)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), status))
        {
            return BadRequest("Invalid order status");
        }

        var service = _orderHandlerFactory.GetHandler(status);
        await service.HandleAsync(id);
        
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder([FromRoute] Guid id)
    {
        var order = await orderService.GetOrderById(id);

        return Ok(
            new ResponseOrderDto
        {
            CustomerId = order.CustomerId,
            DeliveryAddress = order.DeliveryAddress,
            CreatedAt = order.CreatedAt,
            Status = order.Status
        }
        );
    }
    
}