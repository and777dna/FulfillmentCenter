using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using FulfillmentCenter.Services.Implementations;
using FulfillmentCenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentCenter.Controllers;
    
[ApiController]
[Route("/api/shipments")]
public class ShipmentsController(IShipmentService shipmentService) : ControllerBase
{
    private IShipmentService _shipmentService = shipmentService;
    
    
    [HttpPost]
    public IActionResult CreateShipment([FromBody] Shipment shipment)
    {
        _shipmentService.CreateShipment(shipment);
        //TODO review: this invokes the controller action as a plain C# method, bypassing the HTTP pipeline entirely. Should be to call _shipmentService.UpdateShipmentStatus(...) directly instead
        //TODO: to understand why cant we bypass the HTTP pipeline entirely
        _shipmentService.UpdateShipmentStatus(shipment.Id, ShipmentStatus.Shipped);
        return Ok("Shipment has been created.");
    }
    
    //TODO review: you don't need to use $ in the route. It's not valid
    //The route template uses {id} but the method parameter is named shipmentId. These must match for route binding to work. It should be either {shipmentId} in the route or Guid id in the parameter
    [HttpPut("{id}/status")]
    public IActionResult UpdateShipmentStatus([FromQuery] Guid shipmentId,[FromQuery] ShipmentStatus status)
    {
        _shipmentService.UpdateShipmentStatus(shipmentId, status);
        return Ok("Shipment status has been updated.");
    }
    
}