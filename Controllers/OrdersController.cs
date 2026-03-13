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
    private IOrderService _orderService = orderService;
    
    [HttpPost]
    public IActionResult CreateOrder([FromBody] RequestOrderDto orderDto)
    {
        _orderService.CreateOrder(orderDto);
        return Ok("New order has been created.");
    }
    
    [HttpPut("{id}/status")]
    public IActionResult ChangeOrderStatus([FromQuery] Guid id,[FromQuery] OrderStatus status)
    {
        _orderService.UpdateOrderStatus(status, id);
        return Ok("Order status has been created.");
    }
    
    [HttpGet("{id}")]
    public IActionResult GetOrders([FromQuery] Guid id)
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