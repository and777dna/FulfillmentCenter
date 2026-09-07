using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/inventory")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddStock([FromBody] RequestInventoryDto? inventoryDto)
    {
        if (inventoryDto != null)
        {
            await inventoryService.AddStock(inventoryDto, inventoryDto.DistributionCenterId);
            return Ok();
        }

        return BadRequest();
    }
    
    [HttpGet("{centerId}")]
    public async Task<IActionResult> InventoryRemaining([FromRoute] Guid centerId, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var queryParams = new QueryParams()
        {
            Page = page,
            PageSize = pageSize
        };
        var pagedResult = await inventoryService.RemainingsOnTheFulfillmentCenter(centerId, queryParams);
        return Ok(pagedResult);
    }
}