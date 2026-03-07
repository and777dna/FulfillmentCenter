using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
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
    public void CreateOrder(RequestOrderDto orderDto)
    {
        _orderService.CreateOrder(orderDto);
    }
    
    [HttpPut("{id}/status")]
    public void ChangeOrderStatus(Guid id, OrderStatus status)
    {
        _orderService.UpdateOrderStatus(status, id);
    }
    
    [HttpGet("{id}")]
    public ResponseOrderDto GetOrders(Guid id)
    {
        var findOrderById = _orderService.GetOrderById(id);

        return new ResponseOrderDto
        {
            CustomerName = findOrderById.CustomerName,
            DeliveryAddress = findOrderById.DeliveryAddress,
            CreatedAt = findOrderById.CreatedAt,
            Status = findOrderById.Status
        };
    }
}