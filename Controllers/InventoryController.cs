using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.DTOs.Responses;
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
    public void AddStock(RequestInventoryDto inventoryDto, Guid fulfillmentCenterId)
    {
        _inventoryService.AddStock(inventoryDto, fulfillmentCenterId);
    }
    
    [HttpGet("{centerId}")]
    public List<ResponseInventoryDto> InventoryRemaining(Guid centerId)
    {
        //return _inventoryService.RemainingsOnTheFulfillmentCenter(centerId);
        var remainings = _inventoryService.RemainingsOnTheFulfillmentCenter(centerId).ToList();
        List<ResponseInventoryDto> remainingsPdo = remainings.Select(remain => new ResponseInventoryDto
        {
            ProductId = remain.ProductId,
            Quantity = remain.Quantity
        }).ToList();
        return remainingsPdo;
    }
}