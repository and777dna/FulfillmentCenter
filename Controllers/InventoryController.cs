using FulfillmentCenter.Entities;
using FulfillmentCenter.Services.Implementations; //TODO review: The controller should depend on the interface namespace (Services.Interfaces), not the implementation.
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

//TODO review: 
//1. You need to use [ApiController] attribute here. Without it, model binding behaviors (automatic 400 responses, binding source inference, etc.) won't work
//2. Not inheriting from ControllerBase, which means it has no access to HttpContext, Ok(), BadRequest() and so on
[Route("/api/inventory")]
public class InventoryController
{
    //TODO review: _inventoryService is never initialized. If you call any method on it, you will get a null reference exception. It's better to use IInventoryService interface as abstraction layer and testability
    private InventoryService _inventoryService;
    
    //TODO review: Inventory should come from [FromBody] and fulfillmentCenterId likely from [FromQuery] or the route. Without explicit binding attributes, ASP.NET Core won't know where to source these from
    [HttpPost]
    public void AddStock(Inventory inventory, Guid fulfillmentCenterId)
    {
        _inventoryService.AddStock(inventory, fulfillmentCenterId);
    }
    //TODO review: Both the request (Inventory) and response (ICollection<Inventory>) use the raw entity directly instead of DTOs. This is a leaky abstraction and can expose unintended fields or cause serialization issues with navigation properties.
    [HttpGet("{centerId}")]
    public ICollection<Inventory> InventoryRemaining(Guid centerId)
    {
        return _inventoryService.RemainingsOnTheFulfillmentCenter(centerId);
    }
}