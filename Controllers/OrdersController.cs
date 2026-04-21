using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] RequestOrderDto? orderDto)
    {
        if (orderDto != null)
        {
            await orderService.CreateOrder(orderDto);
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
        await orderService.UpdateOrderStatus(status, id);
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