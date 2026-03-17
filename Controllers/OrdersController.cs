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
    public IActionResult CreateOrder([FromBody] RequestOrderDto orderDto)
    {
        _orderService.CreateOrder(orderDto);
        return Ok();
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
        var findOrderById = _orderService.GetOrderById(id).Result;

        return Ok(
            new ResponseOrderDto
        {
            CustomerName = findOrderById.CustomerName,
            DeliveryAddress = findOrderById.DeliveryAddress,
            CreatedAt = findOrderById.CreatedAt,
            Status = findOrderById.Status
        }
        );
    }
    
}