using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[Route("/api/inventory")]
public class InventoryController
{
    private InventoryService _inventoryService;
    
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