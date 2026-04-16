using System.Transactions;
using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/order-item")]
public class OrderItemController(IOrderItemService orderItemService, IInventoryService inventoryService) : ControllerBase
{
    private readonly IOrderItemService _orderItemService = orderItemService;
    private readonly IInventoryService _inventoryService = inventoryService;
    
    [HttpPost]
    public async Task<IActionResult> AddOrderItemToOrder([FromBody] RequestOrderItemDto? orderItemDto, [FromRoute] Guid centerId)
    {
        if (orderItemDto == null) throw new ArgumentNullException(nameof(orderItemDto), "OrderItemDto is null");
        
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await _orderItemService.AddOrderItemToOrder(orderItemDto);
        await _inventoryService.UpdateInventoryProduct(orderItemDto.ProductId, orderItemDto.Quantity, centerId);
        
        scope.Complete();
        return Ok();
    }

}