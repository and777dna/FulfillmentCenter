using FulfillmentCenter.DTOs.Requests;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;

[ApiController]
[Route("/api/shipments")]
public class ShipmentsController(IShipmentService shipmentService) : ControllerBase
{
    private IShipmentService _shipmentService = shipmentService;
    
    [HttpPost]
    public async Task<IActionResult> CreateShipment([FromBody] RequestShipmentDto shipmentDto)
    {
        await _shipmentService.CreateShipment(shipmentDto);
        //TODO review: this invokes the controller action as a plain C# method, bypassing the HTTP pipeline entirely. Should be to call _shipmentService.UpdateShipmentStatus(...) directly instead
        //TODO: to understand why cant we bypass the HTTP pipeline entirely
        return Ok("Shipment has been created.");
    }
   
    [HttpPut("{shipmentId}/status")]
    public async Task<IActionResult> UpdateShipmentStatus([FromRoute] Guid shipmentId,[FromQuery] ShipmentStatus status)
    {
        await _shipmentService.UpdateShipmentStatus(shipmentId, status);
        return Ok("Shipment status has been updated.");
    }
    
}