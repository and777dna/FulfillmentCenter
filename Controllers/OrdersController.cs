using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private IOrderService _orderService = orderService;
    
    [HttpPost] //TODO: to understand why we have inside HttpGet Route id, but not inside HttpPost Route
    public void CreateOrder(Order order)
    {
        _orderService.CreateOrder(order);
    }
    
    [HttpPut("{id}/status")]
    public void ChangeOrderStatus(Guid id, OrderStatus status)
    {
        _orderService.UpdateOrderStatus(status, id);
    }
    
    [HttpGet("{id}")]
    public Order GetOrders(Guid id)
    {
        var findOrderById = _orderService.GetOrderById(id);
        return findOrderById;
    }
    
}