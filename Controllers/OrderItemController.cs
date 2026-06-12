using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/order-item")]
public class OrderItemController(IOrderItemService orderItemService, IInventoryService inventoryService) : ControllerBase
{
    private readonly IOrderItemService _orderItemService = orderItemService;
    
    [HttpPost]
    public async Task<IActionResult> AddOrderItemToOrder([FromBody] RequestOrderItemDto? orderItemDto, [FromRoute] Guid centerId)
    {
        if (orderItemDto == null) throw new ArgumentNullException(nameof(orderItemDto), "OrderItemDto is null");
        
        await _orderItemService.AddOrderItemToOrder(orderItemDto, centerId);
      
        return Ok();
    }

}