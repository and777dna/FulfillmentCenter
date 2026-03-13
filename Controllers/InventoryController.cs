using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/inventory")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    private IInventoryService _inventoryService = inventoryService;
    
    
    [HttpPost]
    public IActionResult AddStock([FromBody] RequestInventoryDto inventoryDto,[FromBody] Guid fulfillmentCenterId)
    {
        _inventoryService.AddStock(inventoryDto, fulfillmentCenterId);
        return Ok("New stock has been added.");
    }
    
    [HttpGet("{centerId}")]
    public IActionResult InventoryRemaining([FromQuery] Guid centerId)
    {
        //return _inventoryService.RemainingsOnTheFulfillmentCenter(centerId);
        var remainings = _inventoryService.RemainingsOnTheFulfillmentCenter(centerId).Result;
        List<ResponseInventoryDto> remainingsPdo = remainings.Select(remain => new ResponseInventoryDto
        {
            ProductId = remain.ProductId,
            Quantity = remain.Quantity
        }).ToList();
        return Ok(remainingsPdo);
    }
}