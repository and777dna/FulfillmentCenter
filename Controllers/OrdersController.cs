using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[Route("/api/orders")]
public class OrdersController : Controller
{
    private OrderService _orderService;
    
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
        var findedOrderById = _orderService.GetOrderById(id);
        return findedOrderById;
    }
    
}