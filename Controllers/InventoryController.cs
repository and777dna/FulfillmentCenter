using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/inventory")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    private readonly IInventoryService _inventoryService = inventoryService;
    
    [HttpPost]
    public async Task<IActionResult> AddStock([FromBody] RequestInventoryDto? inventoryDto)
    {
        if (inventoryDto != null)
        {
            await _inventoryService.AddStock(inventoryDto, inventoryDto.DistributionCenterId);
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpGet("{centerId}")]
    public async Task<IActionResult> InventoryRemaining([FromRoute] Guid centerId)
    {
        var remainingsDto = await _inventoryService.RemainingsOnTheFulfillmentCenter(centerId);
        return Ok(remainingsDto);
    }
}