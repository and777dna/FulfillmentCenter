using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

//TODO review: same problems as in InventoryController
[Route("/api/orders")]
public class OrdersController : Controller
{
    private OrderService _orderService;
    
    [HttpPost] //TODO review: to understand why we have inside HttpGet Route id, but not inside HttpPost Route
    //Should return IActionResult (201 Created or 400 Bad Request)
    public void CreateOrder(Order order)
    {
        _orderService.CreateOrder(order);
    }
    
    //TODO review: should return IActionResult (200 OK or 400 Bad Request)
    [HttpPut("{id}/status")]
    public void ChangeOrderStatus(Guid id, OrderStatus status)
    {
        _orderService.UpdateOrderStatus(status, id);
    }
    
    //TODO review: should return ActionResult<Order> to allow returning 404 Not Found if the order is not found
    [HttpGet("{id}")]
    public Order GetOrders(Guid id)
    {
        //TODO review: typo "findedOrderById" should be "foundOrderById"
        var findedOrderById = _orderService.GetOrderById(id);
        return findedOrderById;
    }
    
}