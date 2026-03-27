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
    private readonly IOrderService _orderService = orderService;
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] RequestOrderDto? orderDto)
    {
        if (orderDto != null)
        {
            await _orderService.CreateOrder(orderDto);
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpPut("{id}/status")]
    public IActionResult ChangeOrderStatus([FromRoute] Guid id,[FromQuery] OrderStatus status)
    {
        _orderService.UpdateOrderStatus(status, id);
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetOrder([FromRoute] Guid id)
    {
        var order = _orderService.GetOrderById(id).Result;

        return Ok(
            new ResponseOrderDto
        {
            CustomerName = order.CustomerName,
            DeliveryAddress = order.DeliveryAddress,
            //CreatedAt = order.CreatedAt,
            Status = order.Status
        }
        );
    }
    
}