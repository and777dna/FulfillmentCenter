using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/inventory")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    private IInventoryService _inventoryService = inventoryService;
    
    [HttpPost]
    public void AddStock(Inventory inventory, Guid fulfillmentCenterId)
    {
        _inventoryService.AddStock(inventory, fulfillmentCenterId);
    }
    
    [HttpGet("{centerId}")]
    public ICollection<Inventory> InventoryRemaining(Guid centerId)
    {
        return _inventoryService.RemainingsOnTheFulfillmentCenter(centerId);
    }
}