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
        _shipmentService.UpdateShipmentStatus(shipmentDto.Id, ShipmentStatus.Shipped);
        return Ok("Shipment has been created.");
    }
    
    //TODO review: you don't need to use $ in the route. It's not valid
    //The route template uses {id} but the method parameter is named shipmentId. These must match for route binding to work. It should be either {shipmentId} in the route or Guid id in the parameter
    [HttpPut("{shipmentId}/status")]
    public async Task<IActionResult> UpdateShipmentStatus([FromRoute] Guid shipmentId,[FromQuery] ShipmentStatus status)
    {
        _shipmentService.UpdateShipmentStatus(shipmentId, status);
        return Ok("Shipment status has been updated.");
    }
    
}